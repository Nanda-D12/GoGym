using ErrorOr;

namespace GoGym.Domain.GymAggregate;

public static class GymErrors
{
    public static readonly Error CannotAddMoreRoomsThanSubscriptionAllow = Error.Validation(
        code: "CannotAddMoreRoomsThanSubscriptionAllows",
        description: "Cannot add more rooms to the Gym than the subscription allows."
    );

    public static readonly Error CannotAddSameRoom = Error.Conflict(
        code: "CannotAddSameRoom",
        description: "Cannot add the same room again to the Gym."
    );
}