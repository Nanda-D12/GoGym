using GoGym.Application.Participants.Commands;
using GoGym.Application.Participants.Queries;
using GoGym.Application.Reservations.Commands;
using GoGym.Contracts.Sessions;
using GoGym.Domain.SessionAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoGym.Api.Controllers;

[Route("partcipants")]
public class ParticipantsController : ApiController
{
    private readonly ISender _sender;

    public ParticipantsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("{participantId:guid}/sessions/{sessionId:guid}/reservation")]
    public async Task<IActionResult> CreateReservation(
        Guid participantId,
        Guid sessionId
    )
    {
        var command = new CreateReservationCommand(participantId, sessionId);

        var createReservationResult = await _sender.Send(command);

        return createReservationResult.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpDelete("{participantId:guid}/sessions/{sessionId:guid}/reservation")]
    public async Task<IActionResult> CancelReservation(Guid participantId, Guid sessionId)
    {
        var command = new CancelReservationCommand(participantId, sessionId);

        var cancelRservationResult = await _sender.Send(command);

        return cancelRservationResult.Match(
            _ => NoContent(),
            Problem
        );
    }

    [HttpGet("{participantId:guid}/sessions")]
    public async Task<IActionResult> ListParticipantSessions(Guid participantId, DateTime? startDateTime = null, DateTime? endDateTime = null)
    {
        var query = new ListParticipantSessionsQuery(participantId, startDateTime, endDateTime);

        var listParticipantSessionsResult = await _sender.Send(query);

        return listParticipantSessionsResult.Match(
            sessions => Ok(sessions.ConvertAll(
                session => new SessionResponse(
                    session.Id,
                    session.Name,
                    session.Description,
                    0,
                    session.MaxParticipants,
                    session.Date.ToDateTime(session.Time.Start),
                    session.Date.ToDateTime(session.Time.End),
                    session.TrainerId
                )
            )),
            Problem
        );
    }
}