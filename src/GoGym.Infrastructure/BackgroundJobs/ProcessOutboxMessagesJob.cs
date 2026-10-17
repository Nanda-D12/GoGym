
using GoGym.Domain.Common;
using GoGym.Infrastructure.Persistence;
using GoGym.Infrastructure.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace GoGym.Infrastructure.BackgroundJobs;

public class ProcessOutboxMessagesJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProcessOutboxMessagesJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GoGymDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

            var messages = await dbContext.Set<OutboxMessaage>()
                                        .Where(m => m.ProcessedOnUtc == null)
                                        .OrderBy(m => m.OccuredOnUtc)
                                        .Take(50)
                                        .ToListAsync(stoppingToken);

            var quarantinedAggregates = new HashSet<Guid>();

            foreach(var message in messages)
            {
                if (quarantinedAggregates.Contains(message.AggregateId))
                {
                    continue;
                }

                try
                {
                    var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                                                    message.Content,
                                                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }
                    );

                    if (domainEvent is not null)
                    {
                        await publisher.Publish(domainEvent, stoppingToken);
                    }

                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch(Exception ex)
                {
                    message.Error = ex.Message;
                    quarantinedAggregates.Add(message.AggregateId);
                }
            }

            if(messages.Count > 0)
            {
                await dbContext.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}