using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.SessionAggregate;

namespace GoGym.Domain.RoomAggregate;

public class Room : AggregateRoot
{
    private readonly List<Guid> _sessionIds = new();
    private readonly Guid _gymId;
    private readonly int _maxDailySessions;
    private readonly Schedule _schedule = Schedule.Empty();
    public string Name { get; }

    public Room(string name, int maxDailySessions, Guid gymId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Name = name;
        _maxDailySessions = maxDailySessions;
        _gymId = gymId;
    }

    public bool HasSession(Guid sessionId)
    {
        return _sessionIds.Contains(sessionId);
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