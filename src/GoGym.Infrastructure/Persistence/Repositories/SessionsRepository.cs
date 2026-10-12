using GoGym.Application.Common.Interfaces;
using GoGym.Domain.GymAggregate;
using GoGym.Domain.SessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class SessionsRepository : ISessionsRepository
{
    private readonly GoGymDbContext _dbContext;

    public SessionsRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddSesssionAsync(Session session)
    {
        await _dbContext.Sessions.AddAsync(session);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Session?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Session>> ListByGymIdAsync(Guid gymId, DateTime? startDateTime = null, DateTime? endDateTime = null)
    {
        var roomsSessions = await _dbContext.Rooms
                                            .AsNoTracking()
                                            .Where(r => r.GymId == gymId)
                                            .SelectMany(r => r.SessionIds)
                                            .ToListAsync();
        
        return await ListByIdsAsync(roomsSessions, startDateTime, endDateTime);
    }

    public async Task<List<Session>> ListByIdsAsync(IReadOnlyList<Guid> sessionIds, DateTime? startDateTime = null, DateTime? endDateTime = null)
    {
        var query = _dbContext.Sessions
                        .AsNoTracking()
                        .Where(s => sessionIds.Contains(s.Id));

        if (startDateTime is not null)
        {
            var startDate = DateOnly.FromDateTime(startDateTime.Value);
            var startTime = TimeOnly.FromDateTime(startDateTime.Value);

            query = query.Where(session => 
                                    session.Date > startDate ||
                                    (session.Date == startDate && session.Time.Start  >= startTime));
        }

        if (endDateTime is not null)
        {
            var endDate = DateOnly.FromDateTime(endDateTime.Value);
            var endTime = TimeOnly.FromDateTime(endDateTime.Value);

            query = query.Where(session => 
                                    session.Date < endDate ||
                                    (session.Date == endDate && session.Time.Start  <= endTime));
        }

        return await query.ToListAsync();
    }

    public async Task RemoveRangeAsync(List<Session> sessions)
    {
        _dbContext.Sessions.RemoveRange(sessions);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSessionAsync(Session session)
    {
        _dbContext.Sessions.Update(session);
        await _dbContext.SaveChangesAsync();
    }
}