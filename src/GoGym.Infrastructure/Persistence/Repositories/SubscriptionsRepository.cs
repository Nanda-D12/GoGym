using GoGym.Application.Common.Interfaces;
using GoGym.Domain.SubscriptionAggregate;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly GoGymDbContext _dbContext;

    public SubscriptionsRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await _dbContext.Subscriptions.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Subscription?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        _dbContext.Subscriptions.Update(subscription);
        await _dbContext.SaveChangesAsync();
    }
}