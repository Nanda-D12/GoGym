using GoGym.Domain.SubscriptionAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task<Subscription?> GetByIdAsync(Guid id);
    Task AddSubscriptionAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
}