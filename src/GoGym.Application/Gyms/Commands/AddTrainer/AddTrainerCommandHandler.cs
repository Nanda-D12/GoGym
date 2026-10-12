using ErrorOr;
using GoGym.Application.Common.Interfaces;
using MediatR;

namespace GoGym.Application.Gyms.Commands.AddTrainer;

public class AddTrainerCommandHandler : IRequestHandler<AddTrainerCommand, ErrorOr<Success>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository;
    private readonly ITrainersRepository _trainersRepository;

    public AddTrainerCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository, ITrainersRepository trainersRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
        _trainersRepository = trainersRepository;
    }

    public async Task<ErrorOr<Success>> Handle(AddTrainerCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if ( subscription is null )
        {
            return Error.NotFound(description: "Subscription not found.");
        }

        var gym = await _gymsRepository.GetByIdAsync(command.GymId);

        if ( gym is null )
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var trainer = await _trainersRepository.GetByIdAsync(command.TrainerId);

        if ( trainer is null )
        {
            return Error.NotFound(description: "Trainer not found.");
        }

        var addTrainerResult = gym.AddTrainer(trainer);

        if ( addTrainerResult.IsError)
        {
            return addTrainerResult.Errors;
        }

        await _gymsRepository.UpdateAsync(gym);

        return Result.Success;
    }
}