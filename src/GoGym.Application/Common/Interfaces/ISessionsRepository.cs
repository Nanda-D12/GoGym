using GoGym.Domain.SessionAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface ISessionsRepository
{
    Task AddSesssionAsync(Session session);
    Task<Session?> GetByIdAsync(Guid id);
    Task UpdateSessionAsync(Session session);
    Task<List<Session>> ListByIdsAsync(
        IReadOnlyList<Guid> sessionIds,
        DateTime? startDateTime = null,
        DateTime? endDateTime = null
    );
    Task<List<Session>> ListByGymIdAsync(
        Guid gymId,
        DateTime? startDateTime = null,
        DateTime? endDateTime = null
    );
    Task RemoveRangeAsync(List<Session> sessions);
}