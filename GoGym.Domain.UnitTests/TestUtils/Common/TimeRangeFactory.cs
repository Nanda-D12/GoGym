using Throw;

namespace GoGym.Domain.UnitTests.TestUtils.Common;

public static class TimeRangeFactory
{
    public static TimeRange CreateFromHours(int startHour, int endHour)
    {
        startHour.Throw()
                .IfGreaterThanOrEqualTo(endHour)
                .IfNegative()
                .IfGreaterThan(23);

        endHour.Throw()
                .IfNegative()
                .IfGreaterThan(24);

        return new TimeRange(start: TimeOnly.MinValue.AddHours(startHour), 
                        end: TimeOnly.MinValue.AddHours(endHour));
    }
}