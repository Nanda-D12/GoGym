using System.Runtime.Intrinsics.X86;
using ErrorOr;
using GoGym.Application.Sessions.Commands.CreateSession;
using GoGym.Application.Sessions.Queries.GetSession;
using GoGym.Contracts.Sessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoGym.Api.Controllers;

[Route("room/{roomId:guid}/sessions")]
public class SessionsController : ApiController
{
    private readonly ISender _sender;

    public SessionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession(CreateSessionRequest request, Guid roomId)
    {
        var command = new CreateSessionCommand(
            roomId,
            request.Name,
            request.Description,
            request.MaxParticipants,
            request.StartDateTime,
            request.EndDateTime,
            request.TrainerId
        );

        var createSessionResult = await _sender.Send(command);

        return createSessionResult.Match(
            session => CreatedAtAction(
                nameof(GetSession),
                new { roomId, SessionId = session.Id},
                new SessionResponse(
                    session.Id,
                    session.Name,
                    session.Description,
                    0,
                    session.MaxParticipants,
                    session.Date.ToDateTime(session.Time.Start),
                    session.Date.ToDateTime(session.Time.End),
                    session.TrainerId
                )
            ),
            Problem
        );
    }

    [HttpGet("{sessionId:guid}")]
    public async Task<IActionResult> GetSession(Guid roomId, Guid sessionId)
    {
        var query = new GetSessionQuery(roomId, sessionId);

        var getSessionResult = await _sender.Send(query);

        return getSessionResult.Match(
            session => Ok(new SessionResponse(
                session.Id,
                session.Name,
                session.Description,
                0,
                session.MaxParticipants,
                session.Date.ToDateTime(session.Time.Start),
                session.Date.ToDateTime(session.Time.End),
                session.TrainerId
            )),
            Problem
        );
    }

}