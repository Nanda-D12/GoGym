using FluentAssertions;
using GoGym.Domain.UnitTests.TestUtils.Gyms;
using GoGym.Domain.UnitTests.TestUtils.Rooms;

namespace GoGym.Domain.UnitTests;

public class GymTests
{
    [Fact]
    public void AddRoom_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // arrange
        var gym = GymFactory.CreateGym();
        var room1 = RoomFactory.CreateRoom(id: Guid.NewGuid());
        var room2 = RoomFactory.CreateRoom(id: Guid.NewGuid());
    
        // act
        var addRoom1Result = gym.AddRoom(room1);
        var addRoom2Result = gym.AddRoom(room2);

    
        // assert
        addRoom1Result.IsError.Should().BeFalse();
        addRoom2Result.IsError.Should().BeTrue();
        addRoom2Result.FirstError.Should().Be(GymErrors.CannotAddMoreRoomsThanSubscriptionAllow);
    }
}