using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.SessionAggregate;

namespace GoGym.Domain.TrainerAggregate;

public class Trainer : AggregateRoot
{
    private readonly Guid _userId;
    private readonly List<Guid> _sessionIds = new();
    private readonly Schedule _schedule = Schedule.Empty();

    public Trainer(Guid userId, Schedule? schedule = null, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _userId = userId;
        _schedule = schedule ?? Schedule.Empty();
        _sessionIds = [];
    }

    public bool IsTimeSlotFree(DateOnly date, TimeRange time)
    {
        return _schedule.CanBookTimeSlot(date, time);
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
        
        _sessionIds.Add(session.Id);
        return Result.Success;
    }

    private Trainer() {}
}