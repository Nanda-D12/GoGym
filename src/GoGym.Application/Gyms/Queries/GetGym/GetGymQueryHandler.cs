using ErrorOr;
using GoGym.Domain.GymAggregate;
using GoGym.Application.Common.Interfaces;
using MediatR;

namespace GoGym.Application.Gyms.Queries.GetGym;

public class GetGymQueryHandler : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository;

    public GetGymQueryHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
    }

    public async Task<ErrorOr<Gym>> Handle(GetGymQuery query, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(query.SubscriptionId);

        if ( subscription is null )
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        if ( !subscription.HasGym(query.GymId))
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var gym = await _gymsRepository.GetByIdAsync(query.GymId);

        if ( gym is null )
        {
            return Error.NotFound(description: "Gym not found.");
        }

        return gym;
    }
}