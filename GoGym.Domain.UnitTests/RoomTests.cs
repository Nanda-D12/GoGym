using FluentAssertions;
using GoGym.Domain.UnitTests.TestConstants;
using GoGym.Domain.UnitTests.TestUtils.Common;
using GoGym.Domain.UnitTests.TestUtils.Rooms;
using GoGym.Domain.UnitTests.TestUtils.Sessions;

namespace GoGym.Domain.UnitTests;

public class RoomTests
{
    [Fact]
    public void ScheduleSession_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // arrange
        var room = RoomFactory.CreateRoom(maxDailySessions: 1);
        var session1 = SessionFactory.CreateSession(id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(id: Guid.NewGuid());

        // act
        var scheduleSession1Result = room.ScheduleSession(session1);
        var scheduleSession2Result = room.ScheduleSession(session2);
    
        // assert
        scheduleSession1Result.IsError.Should().BeFalse();
        scheduleSession2Result.IsError.Should().BeTrue();
        scheduleSession2Result.FirstError.Should().Be(RoomErrors.CannotAddMoreThanMaxDailySessions);
    }

    [Theory]
    [InlineData(6, 8, 7, 9)]
    [InlineData(2, 3, 2, 3)]
    [InlineData(1, 5, 3, 4)]
    [InlineData(4, 6, 3, 5)]
    public void ScheduleSession_WhenSessionOverlapsWithAnotherSession_ShouldFail(
        int startHourSession1,
        int endHourSession1,
        int startHourSession2,
        int endHourSession2
    )
    {
        // arrange
        var room = RoomFactory.CreateRoom(maxDailySessions: 2);
        var session1 = SessionFactory.CreateSession(date: Constants.Session.Date, 
                                                    time: TimeRangeFactory.CreateFromHours(startHourSession1, endHourSession1),
                                                    id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(date: Constants.Session.Date,
                                                    time: TimeRangeFactory.CreateFromHours(startHourSession2, endHourSession2),
                                                     id: Guid.NewGuid());                                                        

        // act
        var scheduleSession1Result = room.ScheduleSession(session1);
        var scheduleSession2Result = room.ScheduleSession(session2);

        // assert
        scheduleSession1Result.IsError.Should().BeFalse();
        scheduleSession2Result.IsError.Should().BeTrue();

        scheduleSession2Result.FirstError.Should().Be(RoomErrors.SessionOverlapWithOtherSession);
    }
}