using System.Reflection.PortableExecutable;
using ErrorOr;

namespace GoGym.Domain.TrainerAggregate;

public static class TrainerErrors
{

    public static readonly Error SessionOverlapWithOtherSession = Error.Validation(
        code: "SessionOverlapWithOtherSession",
        description: "Cannot add a session that overlaps with existing one."
    );

    public static readonly Error CannotAddSameSession = Error.Conflict(
        code: "CannotAddSameSession",
        description: "Session already scheduled."
    );
}