using GoGym.Domain.RoomAggregate;

namespace GoGym.Application.Common.Interfaces;

public interface IRoomsRepository
{
    Task AddRoomAsync(Room room);
    Task<Room?> GetByIdAsync(Guid id);
    Task RemoveAsync(Room room);
    Task UpdateAsync(Room room);
}