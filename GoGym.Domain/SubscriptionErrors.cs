using System.Reflection.PortableExecutable;
using ErrorOr;

namespace GoGym.Domain;

public static class SubscriptionErrors
{
    public static readonly Error CannotAddGymMoreThanSubscriptionAllows = Error.Validation(
        code: "CannotAddGymMoreThanSubscriptionAllows",
        description: "Cannot add more gyms than the subscription allows."
    );

    public static readonly Error CannotAddSameGym = Error.Conflict(
        code: "CannotAddSameGym",
        description: "Gym already added to subscription."
    );
}