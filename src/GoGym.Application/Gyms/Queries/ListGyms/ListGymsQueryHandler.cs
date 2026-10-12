using ErrorOr;
using GoGym.Domain.GymAggregate;
using GoGym.Application.Common.Interfaces;
using MediatR;

namespace GoGym.Application.Gyms.Queries.ListGyms;

public class ListGymsQueryHandler : IRequestHandler<ListGymsQuery, ErrorOr<List<Gym>>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository;

    public ListGymsQueryHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
    }

    public async Task<ErrorOr<List<Gym>>> Handle(ListGymsQuery query, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(query.SubscriptionId);

        if ( subscription is null )
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        return await _gymsRepository.ListGymsBySubscriptionId(subscription.Id);
    }
}