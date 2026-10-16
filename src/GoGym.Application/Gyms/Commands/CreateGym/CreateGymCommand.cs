using ErrorOr;
using GoGym.Domain.GymAggregate;
using MediatR;

namespace GoGym.Application.Gyms.Commands.CreateGym;

public record CreateGymCommand (
    string Name,
    Guid SubscriptionId
) : IRequest<ErrorOr<Gym>>;