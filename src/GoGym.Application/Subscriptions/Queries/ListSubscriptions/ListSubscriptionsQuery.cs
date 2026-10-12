using ErrorOr;
using GoGym.Domain.SubscriptionAggregate;
using MediatR;

namespace GoGym.Application.Subscriptions.Queries.ListSubscriptions;

public record ListSubscriptionsQuery (
    Guid adminId
) : IRequest<ErrorOr<Subscription>>;