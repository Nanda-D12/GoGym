using ErrorOr;

namespace GoGym.Domain;
public class Participant
{
    private readonly Guid _userId;
    private readonly List<Guid> _sessionIds;
    private readonly Schedule _schedule;
    public Guid Id { get; }

    public Participant(Guid userId, Guid? id = null)
    {
        _userId = userId;
        Id = id ?? Guid.NewGuid();
        _schedule = Schedule.Empty();
        _sessionIds = [];
    }

    public ErrorOr<Success> AddSessionToSchedule(Session session)
    {
        // check if session already added
        // check if session overlaps
        // add session

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
        
        return Result.Success;
    }
}