namespace GoGym.Contracts.Subscriptions;

public record SessionResponse (
    Guid Id,
    SubscriptionType SubscriptionType
);