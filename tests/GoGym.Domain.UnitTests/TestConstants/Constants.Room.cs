using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace GoGym.Domain.UnitTests.TestConstants;

public static partial class Constants
{
    public static class Room
    {
        public static readonly string Name = "TestRoom";
        public static readonly Guid Id = Guid.NewGuid();
        public const int MaxDailySessions = 3;
    }
}