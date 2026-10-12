using System.ComponentModel;
using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.SubscriptionAggregate;
using MediatR;

namespace GoGym.Application.Subscriptions.Queries.ListSubscriptions;

public class ListSubscriptionsQueryHandler : IRequestHandler<ListSubscriptionsQuery, ErrorOr<Subscription>>
{
    private readonly IAdminsRepository _adminsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public ListSubscriptionsQueryHandler(IAdminsRepository adminsRepository, ISubscriptionsRepository subscriptionsRepository)
    {
        _adminsRepository = adminsRepository;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(ListSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        var admin = await _adminsRepository.GetByIdAsync(query.adminId);

        if ( admin is null)
        {
            return Error.NotFound(description: "Admin not found.");
        }

        var subscriptionId = admin.SubscriptionId;

        if ( subscriptionId is null)
        {
            return Error.NotFound(description: "Subscription Not Found.");
        }

        var subscription = await _subscriptionsRepository.GetByIdAsync(subscriptionId.Value);

        if ( subscription is null)
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        return subscription;
    }
}