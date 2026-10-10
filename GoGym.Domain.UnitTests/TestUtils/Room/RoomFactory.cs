using GoGym.Domain.UnitTests.TestConstants;

namespace GoGym.Domain.UnitTests.TestUtils.Rooms;

public static class RoomFactory
{
    public static Room CreateRoom(int maxDailySessions = Constants.Room.MaxDailySessions,
                            Guid? gymId = null,
                            Guid? id = null)
    {
        var room = new Room(
            maxDailySessions: maxDailySessions,
            gymId: gymId ?? Constants.Gym.Id,
            id: id ?? Constants.Room.Id
        );
        return room;
    }
}