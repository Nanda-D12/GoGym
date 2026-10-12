using ErrorOr;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Participants.Queries;

public record ListParticipantSessionsQuery (
    Guid ParticipantId,
    DateTime? StartDateTime = null,
    DateTime? EndDateTime = null
) : IRequest<ErrorOr<List<Session>>>;