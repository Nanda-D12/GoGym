using GoGym.Application.Subscriptions.Commands;
using GoGym.Application.Subscriptions.Queries.ListSubscriptions;
using GoGym.Contracts.Subscriptions;
using GoGym.Domain.SubscriptionAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoGym.Api.Controllers;

[Route("subscriptions")]
public class SubscriptionsController : ApiController
{
    private readonly ISender _sender;

    public SubscriptionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("admins/{adminId:guid}")]
    public async Task<IActionResult> ListSubscriptions(Guid adminId)
    {
        var query = new ListSubscriptionsQuery(adminId);

        var listSubscriptionsResult = await _sender.Send(query);

        return listSubscriptionsResult.Match(
            subscription => Ok(new SubscriptionResponse(
                subscription.Id,
                ToDto(subscription.SubscriptionType)
            )),
            Problem
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        if (!Domain.SubscriptionAggregate.SubscriptionType.TryFromName(
            request.SubscriptionType.ToString(),
            out var subscriptionType))
        {
            return Problem("Invalid subscription type", statusCode: StatusCodes.Status400BadRequest);
        }

        var command = new CreateSubscriptionCommand(
                        subscriptionType,
                        request.AdminId
        );

        var createSubscriptionResult = await _sender.Send(command);

        return createSubscriptionResult.Match(
            subscription => CreatedAtAction(
                nameof(ListSubscriptions),
                new { request.AdminId },
                new SubscriptionResponse(
                    subscription.Id,
                    ToDto(subscription.SubscriptionType)
                )
            ),
            Problem
        );
    }

    public static GoGym.Contracts.Subscriptions.SubscriptionType ToDto(GoGym.Domain.SubscriptionAggregate.SubscriptionType subscriptionType)
    {
        return subscriptionType.Name switch
        {
            nameof(Domain.SubscriptionAggregate.SubscriptionType.Free) => Contracts.Subscriptions.SubscriptionType.Free,
            nameof(Domain.SubscriptionAggregate.SubscriptionType.Starter) => Contracts.Subscriptions.SubscriptionType.Starter,
            nameof(Domain.SubscriptionAggregate.SubscriptionType.Pro) => Contracts.Subscriptions.SubscriptionType.Pro,
            _ => throw new InvalidOperationException()
        };
    }
}