using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.SubscriptionAggregate;

namespace GoGym.Domain.AdminAggregate;

public class Admin : AggregateRoot
{
    private readonly Guid _userId;
    public Guid? SubscriptionId { get; private set; }

    public Admin(Guid userId, Guid? subscriptionId = null, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _userId = userId;
        SubscriptionId = subscriptionId;
    }


    public ErrorOr<Success> SetSubscription(Subscription subscription)
    {
        if ( SubscriptionId.HasValue)
        {
            return Error.Conflict(description: "Admin already has an active subscription.");
        }

        SubscriptionId = subscription.Id;

        return Result.Success;
    }

    private Admin (){}
}