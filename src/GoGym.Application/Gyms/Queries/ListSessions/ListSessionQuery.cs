using ErrorOr;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Gyms.Queries.ListSessions;

public record ListSessionQuery (
    Guid SubscriptionId,
    Guid GymId,
    DateTime? StartDateTime,
    DateTime? EndDateTime
) : IRequest<ErrorOr<List<Session>>>;