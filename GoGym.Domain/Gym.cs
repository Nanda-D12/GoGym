using ErrorOr;

namespace GoGym.Domain;

public class Gym
{
    private readonly Guid _id;
    private readonly List<Guid> _roomIds = new();
    private readonly int _maxRooms;
    private readonly Guid _subscriptionId;

    public Gym(int maxRooms, Guid subscriptionId, Guid? id = null)
    {
        _maxRooms = maxRooms;
        _subscriptionId = subscriptionId;
        _id = id ?? Guid.NewGuid();
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