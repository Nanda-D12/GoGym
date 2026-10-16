using GoGym.Application.Common.Interfaces;
using GoGym.Application.Profiles.Common;
using GoGym.Domain.ParticipantAggregate;
using GoGym.Domain.Profiles;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class ParticipantsRepository : IParticipantsRepository
{
    private readonly GoGymDbContext _dbContext;

    public ParticipantsRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddParticipantAsync(Participant participant)
    {
        await _dbContext.Participants.AddAsync(participant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Participant?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Participants.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Profile?> GetProfileByUserIdAsync(Guid userId)
    {
        var participant = await _dbContext.Participants
                        .AsNoTracking()
                        .FirstOrDefaultAsync(participant => participant.UserId == userId);

        return participant is null ? null : new Profile(participant.Id, ProfileType.Participant);
    }

    public async Task UpdateAsync(Participant participant)
    {
        _dbContext.Participants.Update(participant);
        await _dbContext.SaveChangesAsync();
    }
}