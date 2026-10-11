using ErrorOr;

namespace GoGym.Domain;

public class Trainer
{
    private readonly Guid _id;
    private readonly Guid _userId;
    private readonly List<Guid> _sessionIds;
    private readonly Schedule _schedule;

    public Trainer(Guid userId, Schedule? schedule = null, Guid? id = null)
    {
        _userId = userId;
        _schedule = schedule ?? Schedule.Empty();
        _id = id ?? Guid.NewGuid();
        _sessionIds = [];
    }

    public ErrorOr<Success> AddSessionToSchedule(Session session)
    {
        if (_sessionIds.Contains(session.Id))
        {
            return TrainerErrors.CannotAddSameSession;
        }

        var scheduleSessionResult = _schedule.BookTimeSlot(session.Date, session.Time);
        if (scheduleSessionResult.IsError)
        {
            return scheduleSessionResult.FirstError.Type == ErrorType.Conflict ?
                TrainerErrors.SessionOverlapWithOtherSession
                : scheduleSessionResult.Errors;
        }
        
        return Result.Success;
    }
}