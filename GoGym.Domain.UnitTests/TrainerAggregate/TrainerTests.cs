using FluentAssertions;
using GoGym.Domain.TrainerAggregate;
using GoGym.Domain.UnitTests.TestUtils.Common;
using GoGym.Domain.UnitTests.TestUtils.Sessions;
using GoGym.Domain.UnitTests.TestUtils.Trainers;

namespace GoGym.Domain.UnitTests.TrainerAggregate;

public class TrainerTests
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
        var trainer = TrainerFactory.CreateTrainer();
        var session1 = SessionFactory.CreateSession(time: TimeRangeFactory.CreateFromHours(session1StartHour, session1EndHour),
                        id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(time: TimeRangeFactory.CreateFromHours(session2StartHour, session2EndHour),
                        id: Guid.NewGuid());                        

        // act
        var addSession1Result = trainer.AddSessionToSchedule(session1);
        var addSession2Result = trainer.AddSessionToSchedule(session2);

        // assert
        addSession1Result.IsError.Should().BeFalse();
        addSession2Result.IsError.Should().BeTrue();

        addSession2Result.FirstError.Should().Be(TrainerErrors.SessionOverlapWithOtherSession);
    }
}