using ErrorOr;
using GoGym.Domain.GymAggregate;
using MediatR;

namespace GoGym.Application.Gyms.Queries.GetGym;

public record GetGymQuery (
    Guid SubscriptionId,
    Guid GymId
) : IRequest<ErrorOr<Gym>>;