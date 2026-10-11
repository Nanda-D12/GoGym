using ErrorOr;

namespace GoGym.Domain.SessionAggregate;

public static class SessionErrors
{
    public static readonly Error CannotReserveMoreThanParticipantsMax = Error.Validation(
        code: "Session.CannotReserveMoreThanParticipantsMax",
        description: "Cannot reserve more participants in a session than maximum participants"
    );
    
    public static readonly Error CannotReserveSameParticipant = Error.Conflict(
        code: "Session.CannotReserveSameParticipant",
        description: "Cannot reserve the same participant. Participant already reserved."
    );

    public static readonly Error ParticipantNotReserved = Error.NotFound(
        code: "Session.ParticipantNotReserved",
        description: "Participant not reserved in the session."
    );

    public static readonly Error CannotCancelReservationTooCloseToSession = Error.Validation(
        code: "Session.CannotCancelReservationTooCloseToSession",
        description: "Cannot cancel reservation too close to session start time."
    );
}
