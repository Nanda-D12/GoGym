using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.RoomAggregate;

namespace GoGym.Domain.GymAggregate;

public class Gym : AggregateRoot
{
    private readonly List<Guid> _roomIds = new();
    private readonly int _maxRooms;
    public Guid SubscriptionId { get; }

    public Gym(int maxRooms, Guid subscriptionId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _maxRooms = maxRooms;
        SubscriptionId = subscriptionId;
    }


    public ErrorOr<Success> AddRoom(Room room)
    {
        if(_roomIds.Count >= _maxRooms)
        {
            return GymErrors.CannotAddMoreRoomsThanSubscriptionAllow;
        }
        if (_roomIds.Contains(room.Id))
        {
            return GymErrors.CannotAddSameRoom;
        }

        _roomIds.Add(room.Id);
        
        return Result.Success;
    }

    public bool HasRoom(Guid roomId)
    {
        return _roomIds.Contains(roomId);
    }

    public ErrorOr<Success> RemoveRoom(Room room)
    {
        if (!_roomIds.Contains(room.Id))
        {
            return Error.NotFound(description: "Room not found.");
        }

        _roomIds.Remove(room.Id);

        return Result.Success;
    }
}