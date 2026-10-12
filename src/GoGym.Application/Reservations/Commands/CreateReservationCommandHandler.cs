using ErrorOr;
using GoGym.Application.Common.Interfaces;
using MediatR;

namespace GoGym.Application.Reservations.Commands;

public class CreateReservationCommandHanlder : IRequestHandler<CreateReservationCommand, ErrorOr<Success>>
{
    private readonly IParticipantsRepository _participantsRepository;
    private readonly ISessionsRepository _sessionsRepository;

    public CreateReservationCommandHanlder(IParticipantsRepository participantsRepository, ISessionsRepository sessionsRepository)
    {
        _participantsRepository = participantsRepository;
        _sessionsRepository = sessionsRepository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
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

        if (participant.HasReservationForSession(session.Id))
        {
            return Error.Unexpected(description: "Participant already have a reservation to session.");
        }

        if (!participant.IsTimeSlotFree(session.Date, session.Time))
        {
            return Error.Conflict(description: "Participant's calandar is not free the entire session duration.");
        }

        var reserveSpotResult = session.ReserveSpot(participant);
        
        if (reserveSpotResult.IsError)
        {
            return reserveSpotResult.Errors;
        }

        await _sessionsRepository.UpdateSessionAsync(session);

        return Result.Success;
    }
}