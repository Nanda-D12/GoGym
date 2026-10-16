using GoGym.Domain.Common.Interfaces;

namespace GoGym.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}