using FluentAssertions;
using GoGym.Domain.UnitTests.TestUtils.Common;
using GoGym.Domain.UnitTests.TestUtils.Participants;
using GoGym.Domain.UnitTests.TestUtils.Sessions;

namespace GoGym.Domain.UnitTests;

public class ParticipantTests
{
    [Theory]
    [InlineData(6, 9, 7, 8)]
    [InlineData(6, 8, 7, 10)]
    [InlineData(7, 9, 6, 8)]
    [InlineData(6, 8, 6, 8)]
    public void AddSessionToSchedule_WhenSessionOverlapsWithAnotherSession_ShouldFail(
        int session1StartHour,
        int session1EndHour,
        int session2StartHour,
        int session2EndHour
    )
    {
        // arrange
        var participant = ParticipantFactory.CreateParticipant();
        var session1 = SessionFactory.CreateSession(time: TimeRangeFactory.CreateFromHours(session1StartHour, session1EndHour),
                        id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(time: TimeRangeFactory.CreateFromHours(session2StartHour, session2EndHour),
                        id: Guid.NewGuid());                        

    
        // act
        var scheduleSession1Result = participant.AddSessionToSchedule(session1);
        var scheduleSession2Result = participant.AddSessionToSchedule(session2);
    
        // assert
        scheduleSession1Result.IsError.Should().BeFalse();
        scheduleSession2Result.IsError.Should().BeTrue();

        scheduleSession2Result.FirstError.Should().Be(ParticipantErrors.SessionOverlapWithOtherSession);
    }
}