using System.Runtime.CompilerServices;
using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.SessionAggregate;

namespace GoGym.Domain.ParticipantAggregate;
public class Participant : AggregateRoot
{
    private readonly Guid _userId;
    private readonly Schedule _schedule;
    public List<Guid> SessionIds { get; }

    public Participant(Guid userId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _userId = userId;
        _schedule = Schedule.Empty();
        SessionIds = [];
    }


    public ErrorOr<Success> AddSessionToSchedule(Session session)
    {

        if (SessionIds.Contains(session.Id))
        {
            return ParticipantErrors.CannotAddSameSession;
        }

        var scheduleSessionResult = _schedule.BookTimeSlot(session.Date, session.Time);
        if (scheduleSessionResult.IsError )
        {
            return scheduleSessionResult.FirstError.Type == ErrorType.Conflict ?
                ParticipantErrors.SessionOverlapWithOtherSession
                : scheduleSessionResult.Errors;
        }

        SessionIds.Add(session.Id);
        
        return Result.Success;
    }

    public bool HasReservationForSession(Guid sessionId)
    {
        return SessionIds.Contains(sessionId);
    }

    public bool IsTimeSlotFree(DateOnly date, TimeRange time)
    {
        return _schedule.CanBookTimeSlot(date, time);
    }    
    
    public ErrorOr<Success> RemoveFromSchedule(Session session)
    {
        if (!HasReservationForSession(session.Id))
        {
            return Error.NotFound(description: "Session not found.");
        }

        var removeBookingResult = _schedule.RemoveBooking(session.Date, session.Time);

        if (removeBookingResult.IsError)
        {
            return removeBookingResult.Errors;
        }

        SessionIds.Remove(session.Id);

        return Result.Success;
    }
}