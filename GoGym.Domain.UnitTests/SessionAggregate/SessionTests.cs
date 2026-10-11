using FluentAssertions;
using GoGym.Domain.SessionAggregate;
using GoGym.Domain.UnitTests.TestConstants;
using GoGym.Domain.UnitTests.TestUtils.Participants;
using GoGym.Domain.UnitTests.TestUtils.Services;
using GoGym.Domain.UnitTests.TestUtils.Sessions;

namespace GoGym.Domain.UnitTests.SessionAggregate;

public class SessionTests
{
    [Fact]
    public void ReserveSpot_WhenNoMoreRoom_ReservationShouldFailReservation()
    {
        // Arrange
        var session = SessionFactory.CreateSession(maxParticipants: 1);
        var participant1 = ParticipantFactory.CreateParticipant(userId: Guid.NewGuid(),
                                                                id: Guid.NewGuid());

        var participant2 = ParticipantFactory.CreateParticipant(userId: Guid.NewGuid(),
                                                                id: Guid.NewGuid());                                                                
        // Act
        var reserveParticipant1Result = session.ReserveSpot(participant1);
        var reserveParticipant2Result = session.ReserveSpot(participant2);
        
    
        // Assert
        reserveParticipant1Result.IsError.Should().BeFalse();
        reserveParticipant2Result.IsError.Should().BeTrue();
        reserveParticipant2Result.FirstError.Should().Be(SessionErrors.CannotReserveMoreThanParticipantsMax);
    }

    [Fact]
    public void CancelReservation_WhenCancellationTooCloseToSession_ShouldFailCancelReservation()
    {
        // Arrange
        var session = SessionFactory.CreateSession(maxParticipants: 2,
                                        date: Constants.Session.Date,
                                        time: Constants.Session.Time);
                                        
        var participant = ParticipantFactory.CreateParticipant();
        session.ReserveSpot(participant);

        var cancellationTime = Constants.Session.Date.ToDateTime(TimeOnly.MinValue);

        // Act
        var cancelReservatoinResult = session.CancelReservation(participant, new TestDateTimeProvider(fixedDateTime: cancellationTime));

        // Assert
        cancelReservatoinResult.IsError.Should().BeTrue();
        cancelReservatoinResult.FirstError.Should().Be(SessionErrors.CannotCancelReservationTooCloseToSession);
    }
}