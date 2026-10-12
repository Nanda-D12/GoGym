using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.Common.Interfaces;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.ParticipantAggregate;

namespace GoGym.Domain.SessionAggregate;

public class Session : AggregateRoot
{
    private readonly Guid _trainerId;
    private readonly List<Reservation>  _reservations = new();
    private readonly int _maxParticipants;
    public string Name { get; }
    public string Description { get; }
    public Guid RoomId { get; }
    public DateOnly Date { get; }
    public TimeRange Time { get; }

    public Session(string name, string description, Guid roomId, int maxParticipants, Guid trainerId, DateOnly date, TimeRange time, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Name = name;
        Description = description;
        RoomId = roomId;
        _maxParticipants = maxParticipants;
        _trainerId = trainerId;
        Date = date;
        Time = time;
    }

    public ErrorOr<Success> CancelReservation(Participant participant, IDateTimeProvider dateTimeProvider)
    {
        if (IsSessionTooCloseToCancel(dateTimeProvider.UtcNow))
        {
            return SessionErrors.CannotCancelReservationTooCloseToSession;
        }

        var reservation = _reservations.Find(res => res.ParticipantId == participant.Id);
        if (reservation is null)
        {
            return SessionErrors.ParticipantNotReserved;
        }

        _reservations.Remove(reservation);

        return Result.Success;
    }

    private bool IsSessionTooCloseToCancel(DateTime utcNow)
    {
        const int MinHours = 24;

        return (Date.ToDateTime(Time.Start) - utcNow).TotalHours < MinHours;
    }

    public ErrorOr<Success> ReserveSpot(Participant participant)
    {
        if (_reservations.Count >= _maxParticipants)
        {
            return SessionErrors.CannotReserveMoreThanParticipantsMax;
        }

        var reservation = _reservations.Find(res => res.ParticipantId == participant.Id);

        if (reservation is not null)
        {
            return SessionErrors.CannotReserveSameParticipant;
        }

        _reservations.Add(new Reservation(participant.Id));

        return Result.Success;
    }
}