using System.ComponentModel.DataAnnotations;
using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.Common.ValueObjects;

namespace GoGym.Domain;

public class Schedule : Entity
{
    private readonly Dictionary<DateOnly, List<TimeRange>> _calendar = new();

    private Schedule(Dictionary<DateOnly, List<TimeRange>>? calendar = null, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _calendar = calendar ?? [];
    }

    public static Schedule Empty()
    {
        return new Schedule(id: Guid.NewGuid());
    }

    internal bool CanBookTimeSlot(DateOnly date, TimeRange time)
    {
        if (!_calendar.TryGetValue(date, out var timeSlots))
        {
            return true;
        }

        return !timeSlots.Any(slot => slot.OverlapsWith(time));
    }

    internal ErrorOr<Success> BookTimeSlot(DateOnly date, TimeRange time)
    {
        if(!_calendar.TryGetValue(date, out var timeslots))
        {
            _calendar[date] = [time];
            return Result.Success;
        }
        if(timeslots.Any(slot => time.OverlapsWith(slot)))
        {
            return Error.Conflict();
        }

        timeslots.Add(time);

        return Result.Success;
    }

}