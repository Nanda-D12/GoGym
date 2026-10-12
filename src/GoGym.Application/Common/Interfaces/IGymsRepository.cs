using GoGym.Domain.GymAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface IGymsRepository
{
    Task<Gym?> GetByIdAsync(Guid id);
    Task<List<Gym>> ListGymsBySubscriptionId(Guid subscriptionid);
    Task UpdateAsync(Gym gym);
}