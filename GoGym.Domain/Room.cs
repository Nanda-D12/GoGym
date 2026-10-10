using ErrorOr;

namespace GoGym.Domain;

public class Room
{
    private readonly List<Guid> _sessionIds = new();
    private readonly Guid _gymId;
    private readonly int _maxDailySessions;
    private readonly Schedule _schedule = Schedule.Empty();
    public Guid Id { get; }

    public Room(int maxDailySessions, Guid gymId, Guid? id = null)
    {
        _maxDailySessions = maxDailySessions;
        _gymId = gymId;
        Id = id ?? Guid.NewGuid();
    }

    public ErrorOr<Success> ScheduleSession(Session session)
    {
        Console.WriteLine($"Session Count: {_sessionIds.Count} and Max: {_maxDailySessions}");
        if (_sessionIds.Contains(session.Id))
        {
            return RoomErrors.CannotAddSameSession;
        }
        if (_sessionIds.Count >= _maxDailySessions)
        {
            return RoomErrors.CannotAddMoreThanMaxDailySessions;
        }

        var bookSlotResult = _schedule.BookTimeSlot(session.Date, session.Time);

        if (bookSlotResult.IsError && bookSlotResult.FirstError.Type == ErrorType.Conflict)
        {
            return RoomErrors.SessionOverlapWithOtherSession;
        }

        _sessionIds.Add(session.Id);

        return Result.Success;
    }
}