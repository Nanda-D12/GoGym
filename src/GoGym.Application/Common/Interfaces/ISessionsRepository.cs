using GoGym.Domain.SessionAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface ISessionsRepository
{
    Task AddSesssionAsync(Session session);
    Task UpdateSessionAsync(Session session);
}