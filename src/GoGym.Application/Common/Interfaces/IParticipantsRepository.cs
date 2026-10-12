using GoGym.Domain.ParticipantAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface IParticipantsRepository
{
    Task AddParticipantAsync(Participant participant);
    Task<Participant?> GetByIdAsync(Guid id);
    Task UpdateAsync(Participant participant);
}