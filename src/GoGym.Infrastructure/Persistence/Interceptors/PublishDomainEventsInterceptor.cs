using System.Text.Json.Serialization;
using GoGym.Domain.Common;
using GoGym.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace GoGym.Infrastructure.Persistence.Interceptors;

public class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = dbContext.ChangeTracker
                                      .Entries<AggregateRoot>()
                                      .Select(x => x.Entity)
                                      .SelectMany(aggregateRoot =>
                                      {
                                          var domainEvents = aggregateRoot.PopDomainEvents();

                                         return domainEvents.Select(eventData => new
                                         {
                                             AggregateId = aggregateRoot.Id,
                                             Event = eventData
                                         });
                                      })
                                      .Select(x => new OutboxMessaage
                                      {
                                          Id = Guid.NewGuid(),
                                          AggregateId = x.AggregateId,
                                          Type = x.Event.GetType().Name,
                                          Content = JsonConvert.SerializeObject(
                                            x.Event,
                                            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }
                                          ),
                                          OccuredOnUtc = DateTime.UtcNow
                                      })
                                      .ToList();

        dbContext.Set<OutboxMessaage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}