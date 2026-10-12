using ErrorOr;
using GoGym.Domain.SubscriptionAggregate;
using MediatR;

namespace GoGym.Application.Subscriptions.Commands;

public record CreateSubscriptionCommand (
    SubscriptionType SubscriptionType,
    Guid AdminId
) : IRequest<ErrorOr<Created>>;