using ErrorOr;
using Throw;

namespace GoGym.Domain.Common.ValueObjects;

public class TimeRange : ValueObject
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start.Throw().IfGreaterThanOrEqualTo(end);
        End = end;
    }

    public static ErrorOr<TimeRange> FromDateTimes(DateTime start, DateTime end)
    {
        if(start.Date != end.Date)
        {
            Error.Validation(description: "Start and end date must be on the same day.");
        }

        if (start >= end)
        {
            Error.Validation(description: "End time must be greater than end time");
        }

        return new TimeRange(start: TimeOnly.FromDateTime(start),
                             end: TimeOnly.FromDateTime(end));
    }
    public bool OverlapsWith(TimeRange otherRange)
    {
        return Start <= otherRange.End && End >= otherRange.Start;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}