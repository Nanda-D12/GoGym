using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.RoomAggregate;
using GoGym.Domain.TrainerAggregate;

namespace GoGym.Domain.GymAggregate;

public class Gym : AggregateRoot
{
    private readonly int _maxRooms;
    private readonly List<Guid> _trainerIds = new();
    public string Name { get; } = string.Empty;
    public Guid SubscriptionId { get; }

    public List<Guid> RoomIds { get; } = new();

    public Gym(string name, int maxRooms, Guid subscriptionId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Name = name;
        _maxRooms = maxRooms;
        SubscriptionId = subscriptionId;
    }


    public ErrorOr<Success> AddRoom(Room room)
    {
        if(RoomIds.Count >= _maxRooms)
        {
            return GymErrors.CannotAddMoreRoomsThanSubscriptionAllow;
        }
        if (RoomIds.Contains(room.Id))
        {
            return GymErrors.CannotAddSameRoom;
        }

        RoomIds.Add(room.Id);
        
        return Result.Success;
    }

    public bool HasRoom(Guid roomId)
    {
        return RoomIds.Contains(roomId);
    }

    public ErrorOr<Success> RemoveRoom(Room room)
    {
        if (!RoomIds.Contains(room.Id))
        {
            return Error.NotFound(description: "Room not found.");
        }

        RoomIds.Remove(room.Id);

        return Result.Success;
    }

    public ErrorOr<Success> AddTrainer(Trainer trainer)
    {
        if (_trainerIds.Contains(trainer.Id))
        {
            return Error.Conflict(description: "Trainer already assigned to gym");
        }

        _trainerIds.Add(trainer.Id);

        return Result.Success;
    }

    private Gym() {}
}