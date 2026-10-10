using System.Reflection.PortableExecutable;
using ErrorOr;

namespace GoGym.Domain;

public static class RoomErrors
{
    public static readonly Error CannotAddMoreThanMaxDailySessions = Error.Validation(
        code: "CannotAddMoreThanMaxDailySessions",
        description: "Cannot add more session than the max sessions allowed."
    );

    public static readonly Error SessionOverlapWithOtherSession = Error.Validation(
        code: "SessionOverlapWithOtherSession",
        description: "Cannot add a session that overlaps with existing one."
    );

    public static readonly Error CannotAddSameSession = Error.Conflict(
        code: "CannotAddSameSession",
        description: "Session already added to room."
    );
}