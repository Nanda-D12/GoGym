using ErrorOr;
using MediatR;

namespace GoGym.Application.Gyms.Commands.AddTrainer;

public record AddTrainerCommand (
    Guid SubscriptionId,
    Guid GymId,
    Guid TrainerId
) : IRequest<ErrorOr<Success>>;