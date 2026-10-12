using ErrorOr;
using MediatR;

namespace GoGym.Application.Reservations.Commands;

public record CreateReservationCommand(
    Guid ParticipantId,
    Guid SessionId
) : IRequest<ErrorOr<Success>>;