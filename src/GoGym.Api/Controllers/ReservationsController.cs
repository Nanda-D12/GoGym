using ErrorOr;
using GoGym.Application.Reservations.Commands;
using GoGym.Contracts.Reservations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoGym.Api.Controllers;

[Route("sessions/{sessionId:guid}/reservations")]
public class ReservationsController : ApiController
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(CreateReservationRequest request, Guid sessionId)
    {
        var command = new CreateReservationCommand(request.ParticipantId, sessionId);

        var createReservationResult = await _sender.Send(command);

        return createReservationResult.Match(
            _ => NoContent(),
            Problem
        );
    }
}