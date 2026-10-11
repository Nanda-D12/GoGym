
using GoGym.Domain.Common.Interfaces;

namespace GoGym.Domain.UnitTests.TestUtils.Services;

public class TestDateTimeProvider : IDateTimeProvider
{
    private readonly DateTime? _fixedDateTime;

    public TestDateTimeProvider(DateTime? fixedDateTime = null)
    {
        _fixedDateTime = fixedDateTime;
    }

    public DateTime UtcNow => _fixedDateTime ?? DateTime.UtcNow;
}