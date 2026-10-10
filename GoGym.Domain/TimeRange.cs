using Throw;

namespace GoGym.Domain;

public class TimeRange
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        Start = start.Throw().IfGreaterThanOrEqualTo(end);
        End = end;
    }

    public bool OverlapsWith(TimeRange otherRange)
    {
        return Start <= otherRange.End && End >= otherRange.Start;
    }
}