using ErrorOr;

namespace GoGym.Domain;

public class Session
{
    private readonly Guid _trainerId;
    private readonly List<Guid>  _participantIds = new();
    private readonly int _maxParticipants;

    public Guid Id { get; }

    public DateOnly Date { get; }

    public TimeRange Time { get; }

    public Session(int maxParticipants, Guid trainerId, DateOnly date, TimeRange time, Guid? id = null)
    {
        _maxParticipants = maxParticipants;
        _trainerId = trainerId;
        Date = date;
        Time = time;
        Id = id ?? Guid.NewGuid();
    }

    public ErrorOr<Success> CancelReservation(Participant participant, IDateTimeProvider dateTimeProvider)
    {
        if (IsSessionTooCloseToCancel(dateTimeProvider.UtcNow))
        {
            return SessionErrors.CannotCancelReservationTooCloseToSession;
        }

        if (!_participantIds.Contains(participant.Id))
        {
            return SessionErrors.ParticipantNotReserved;
        }

        _participantIds.Remove(participant.Id);

        return Result.Success;
    }

    private bool IsSessionTooCloseToCancel(DateTime utcNow)
    {
        const int MinHours = 24;

        return (Date.ToDateTime(Time.Start) - utcNow).TotalHours < MinHours;
    }

    public ErrorOr<Success> ReserveSpot(Participant participant)
    {
        if (_participantIds.Count >= _maxParticipants)
        {
            return SessionErrors.CannotReserveMoreThanParticipantsMax;
        }

        if (_participantIds.Contains(participant.Id))
        {
            return SessionErrors.CannotReserveSameParticipant;
        }

        _participantIds.Add(participant.Id);

        return Result.Success;
    }
}