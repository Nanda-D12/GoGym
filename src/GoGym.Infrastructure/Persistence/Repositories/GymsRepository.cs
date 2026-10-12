using GoGym.Application.Common.Interfaces;
using GoGym.Domain.GymAggregate;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class GymsRepository : IGymsRepository
{
    private readonly GoGymDbContext _dbContext;

    public GymsRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Gym?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Gyms.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<Gym>> ListGymsBySubscriptionId(Guid subscriptionid)
    {
        return await _dbContext.Gyms
                        .Where(g => g.SubscriptionId == subscriptionid)
                        .ToListAsync();
    }

    public async Task UpdateAsync(Gym gym)
    {
        _dbContext.Gyms.Update(gym);
        await _dbContext.SaveChangesAsync();
    }
}