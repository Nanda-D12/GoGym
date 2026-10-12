using GoGym.Domain.GymAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface IGymsRepository
{
    Task<Gym?> GetByIdAsync(Guid id);
    Task UpdateAsync(Gym gym);
}