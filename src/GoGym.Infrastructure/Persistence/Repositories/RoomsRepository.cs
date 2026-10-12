using GoGym.Application.Common.Interfaces;
using GoGym.Domain.GymAggregate;
using GoGym.Domain.RoomAggregate;
using Microsoft.EntityFrameworkCore;

namespace GoGym.Infrastructure.Persistence.Repositories;

public class RoomsRepository : IRoomsRepository
{
    private readonly GoGymDbContext _dbContext;

    public RoomsRepository(GoGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRoomAsync(Room room)
    {
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Room>> ListByGymIdAsync(Guid gymId)
    {
        return await _dbContext.Rooms
                                .AsNoTracking()
                                .Where(r => r.GymId == gymId)
                                .ToListAsync();
    }

    public async Task RemoveAsync(Room room)
    {
        _dbContext.Rooms.Remove(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _dbContext.Rooms.Update(room);
        await _dbContext.SaveChangesAsync();
    }
}