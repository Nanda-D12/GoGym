using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.GymAggregate;
using MediatR;

namespace GoGym.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, ErrorOr<Created>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public CreateGymCommandHandler(ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateGymCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if ( subscription is null )
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        var gym = new Gym(name: command.Name, 
                          subscriptionId: command.SubscriptionId, 
                          maxRooms: subscription.GetMaxRooms());

        var addGymResult = subscription.AddGym(gym);

        if (addGymResult.IsError)
        {
            return addGymResult.Errors;
        }

        await _subscriptionsRepository.UpdateAsync(subscription);

        return Result.Created;
    }
}