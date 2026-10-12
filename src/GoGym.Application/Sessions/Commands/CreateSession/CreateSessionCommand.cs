using ErrorOr;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid RoomId,
    string Name,
    string Description,
    int MaxParticipants,
    DateTime StartDateTime,
    DateTime EndDateTime,
    Guid TrainerId
) : IRequest<ErrorOr<Session>>;