using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.SubscriptionAggregate;
using MediatR;

namespace GoGym.Application.Subscriptions.Commands;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly IAdminsRepository _adminsRepository;

    public CreateSubscriptionCommandHandler(IAdminsRepository adminsRepository)
    {
        _adminsRepository = adminsRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var admin = await _adminsRepository.GetByIdAsync(command.AdminId);

        if ( admin is null)
        {
            return Error.NotFound(description: "Admin not found.");
        }

        var subscription = new Subscription(command.SubscriptionType, command.AdminId);

        var setSubscriptionResult = admin.SetSubscription(subscription);

        if ( setSubscriptionResult.IsError )
        {
            return setSubscriptionResult.Errors;
        }

        await _adminsRepository.UpdateAsync(admin);

        return subscription;
    }
}