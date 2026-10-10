using System.Globalization;
using ErrorOr;

namespace GoGym.Domain;

public class Schedule
{
    private readonly Dictionary<DateOnly, List<TimeRange>> _calendar = new();
    private readonly Guid _id;

    private Schedule(Dictionary<DateOnly, List<TimeRange>>? calendar = null, Guid? id = null)
    {
        _calendar = calendar ?? new();
        _id = id ?? Guid.NewGuid();
    }

    public static Schedule Empty()
    {
        return new Schedule(id: Guid.NewGuid());
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