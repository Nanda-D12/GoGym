using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Gyms.Queries.ListSessions;

public class ListSessionQueryHandler : IRequestHandler<ListSessionQuery, ErrorOr<List<Session>>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository;
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public ListSessionQueryHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository, ISessionsRepository sessionsRepository, IRoomsRepository roomsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
        _sessionsRepository = sessionsRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<List<Session>>> Handle(ListSessionQuery query, CancellationToken cancellationToken)
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

        return await _sessionsRepository.ListByGymIdAsync(query.GymId, query.StartDateTime, query.EndDateTime);
    }
}