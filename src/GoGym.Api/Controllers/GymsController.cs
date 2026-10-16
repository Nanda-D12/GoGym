using ErrorOr;
using GoGym.Application.Gyms.Commands.AddTrainer;
using GoGym.Application.Gyms.Commands.CreateGym;
using GoGym.Application.Gyms.Queries.GetGym;
using GoGym.Application.Gyms.Queries.ListGyms;
using GoGym.Application.Gyms.Queries.ListSessions;
using GoGym.Contracts.Gyms;
using GoGym.Contracts.Sessions;
using GoGym.Domain.GymAggregate;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoGym.Api.Controllers;

[Microsoft.AspNetCore.Mvc.Route("subscriptions/{subscriptionId:guid}/gyms")]
public class GymsController : ApiController
{
    private readonly ISender _sender;

    public GymsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGym(CreateGymRequest request, Guid subscriptionId)
    {
        var command = new CreateGymCommand(request.Name, subscriptionId);

        var createGymResult = await _sender.Send(command);

        return createGymResult.Match(
            gym => CreatedAtAction(
                    nameof(GetGym),
                    new { subscriptionId, GymId = gym.Id},
                    new GymResponse(gym.Id, gym.Name)
            ),
            Problem
        );
    }

    [HttpGet("{gymId: guid}")]
    public async Task<IActionResult> GetGym(Guid gymId, Guid subscriptionId)
    {
        var query = new GetGymQuery(subscriptionId, gymId);

        var getGymResult = await _sender.Send(query);

        return getGymResult.Match(
            gym => Ok(new GymResponse(gym.Id, gym.Name)),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> ListGyms(Guid subscriptionId)
    {
        var query = new ListGymsQuery(subscriptionId);

        var listGymResult = await _sender.Send(query);

        return listGymResult.Match(
            gyms => Ok(gyms.ConvertAll(gym => new GymResponse(gym.Id, gym.Name))),
            Problem
        );
    }

    [HttpPost("{gymId:guid}/trainer")]
    public async Task<IActionResult> AddTrainer(AddTrainerRequest request, Guid gymId, Guid subscriptionId)
    {
        var command = new AddTrainerCommand(subscriptionId, gymId, request.TrainerId);

        var addTrainerResult = await _sender.Send(command);

        return addTrainerResult.Match(
            _ => Ok(), Problem
        );
    }

    [HttpGet("{gymId:guid}/sessions")]
    public async Task<IActionResult> ListSessions(Guid subscriptionId, 
                                                  Guid gymId, 
                                                  DateTime? startDateTime = null,
                                                  DateTime? endDateTime = null)
    {
        var query = new ListSessionQuery(subscriptionId, gymId, startDateTime, endDateTime);

        var listSessionsResult = await _sender.Send(query);

        return listSessionsResult.Match(
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