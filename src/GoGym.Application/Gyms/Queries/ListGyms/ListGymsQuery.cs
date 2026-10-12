using ErrorOr;
using GoGym.Domain.GymAggregate;
using MediatR;
using OneOf.Types;

namespace GoGym.Application.Gyms.Queries.ListGyms;

public record ListGymsQuery (
    Guid SubscriptionId
) : IRequest<ErrorOr<List<Gym>>>;