using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.RoomAggregate;

namespace GoGym.Domain.GymAggregate;

public class Gym : AggregateRoot
{
    private readonly List<Guid> _roomIds = new();
    private readonly int _maxRooms;
    private readonly Guid _subscriptionId;

    public Gym(int maxRooms, Guid subscriptionId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _maxRooms = maxRooms;
        _subscriptionId = subscriptionId;
    }


    public ErrorOr<Success> AddRoom(Room room)
    {
        // check if max rooms is filled
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
}