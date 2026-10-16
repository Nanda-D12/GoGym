namespace GoGym.Contracts.Subscriptions;

public record SubscriptionResponse (
    Guid Id,
    SubscriptionType SubscriptionType
);