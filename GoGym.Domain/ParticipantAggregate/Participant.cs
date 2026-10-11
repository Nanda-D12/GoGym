using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.SessionAggregate;

namespace GoGym.Domain.ParticipantAggregate;
public class Participant : AggregateRoot
{
    private readonly Guid _userId;
    private readonly List<Guid> _sessionIds;
    private readonly Schedule _schedule;

    public Participant(Guid userId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _userId = userId;
        _schedule = Schedule.Empty();
        _sessionIds = [];
    }

    public ErrorOr<Success> AddSessionToSchedule(Session session)
    {

        if (_sessionIds.Contains(session.Id))
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

        _sessionIds.Add(session.Id);
        
        return Result.Success;
    }
}