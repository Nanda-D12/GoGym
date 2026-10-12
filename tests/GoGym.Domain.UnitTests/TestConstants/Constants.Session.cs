using System.Security.Cryptography;
using FluentAssertions.Equivalency.Steps;
using GoGym.Domain.Common.ValueObjects;

namespace GoGym.Domain.UnitTests.TestConstants;

public static partial class Constants
{
    public static class Session
    {
        public static readonly string Name = "TestSession";
        public static readonly string Description = "Test Session Description";
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly DateOnly Date = DateOnly.FromDateTime(DateTime.UtcNow);
        public static readonly TimeOnly StartTime = TimeOnly.MinValue.AddHours(7);
        public static readonly TimeOnly EndTime = TimeOnly.MinValue.AddHours(8);
        public static readonly TimeRange Time = new(StartTime, EndTime);
        public const int MaxParticipants = 10;

    }
}