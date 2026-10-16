using GoGym.Application.Profiles.Common;
using GoGym.Domain.TrainerAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface ITrainersRepository
{
    Task AddTrainerAsync(Trainer trainer);
    Task<Trainer?> GetByIdAsync(Guid id);
    Task RemoveAsync(Trainer trainer);
    Task UpdateAsync(Trainer trainer);
    Task<Profile?> GetProfileByUserIdAsync(Guid userId);
}