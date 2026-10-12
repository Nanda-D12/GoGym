using ErrorOr;
using MediatR;

namespace GoGym.Application.Participants.Commands;

public record CancelReservationCommand (
    Guid ParticipantId,
    Guid SessionId
) : IRequest<ErrorOr<Deleted>>;