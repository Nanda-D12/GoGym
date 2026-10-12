using ErrorOr;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Sessions.Queries.GetSession;

public record GetSessionQuery(
    Guid RoomId,
    Guid SessionId
) : IRequest<ErrorOr<Session>>;