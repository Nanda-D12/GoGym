using GoGym.Domain.SubscriptionAggregate;
using GoGym.Domain.UnitTests.TestConstants;

namespace GoGym.Domain.UnitTests.TestUtils.Subscriptions;

public static class SubscriptionFactory
{
    public static Subscription CreateSubscription(SubscriptionType? subscriptionType = null,
                                                    Guid? adminId = null,
                                                    Guid? id = null)
    {
        var subscription = new Subscription(
                                subscriptionType: subscriptionType ?? Constants.Subscription.SubscriptionTypeFreeTier,
                                adminId: adminId ?? Constants.Admin.Id,
                                id: id ?? Constants.Subscription.Id
        );

        return subscription;
    }
}