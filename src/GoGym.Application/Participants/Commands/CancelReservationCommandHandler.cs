using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.Common.Interfaces;
using MediatR;

namespace GoGym.Application.Participants.Commands;

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, ErrorOr<Deleted>>
{
    private readonly IParticipantsRepository _participantsRepository;
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelReservationCommandHandler(IParticipantsRepository participantsRepository, ISessionsRepository sessionsRepository, IDateTimeProvider dateTimeProvider)
    {
        _participantsRepository = participantsRepository;
        _sessionsRepository = sessionsRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Deleted>> Handle(CancelReservationCommand command, CancellationToken cancellationToken)
    {
        var participant = await _participantsRepository.GetByIdAsync(command.ParticipantId);

        if (participant is null)
        {
            return Error.NotFound(description: "Participant not found.");
        }

        var session = await _sessionsRepository.GetByIdAsync(command.SessionId);

        if (session is null)
        {
            return Error.NotFound(description: "Session not found.");
        }

        if (!participant.HasReservationForSession(session.Id))
        {
            return Error.Unexpected(description: "Participant expected to have reservation to session.");
        }

        var cancelReservationResult = session.CancelReservation(participant, _dateTimeProvider);

        if (cancelReservationResult.IsError)
        {
            return cancelReservationResult.Errors;
        }

        await _sessionsRepository.UpdateSessionAsync(session);

        return Result.Deleted;
    }
}