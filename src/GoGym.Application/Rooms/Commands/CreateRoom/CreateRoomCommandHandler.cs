using System.Reflection.Metadata.Ecma335;
using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.RoomAggregate;
using MediatR;

namespace GoGym.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subsriptionsRepository;

    public CreateRoomCommandHandler(IGymsRepository gymsRepository, ISubscriptionsRepository subsriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _subsriptionsRepository = subsriptionsRepository;
    }

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var subscription = await _subsriptionsRepository.GetByIdAsync(gym.SubscriptionId);

        if (subscription is null)
        {
            return Error.Unexpected(description: "Subscription not found.");
        }

        var room = new Room(
                        name: command.Name,
                        maxDailySessions: subscription.GetMaxDailySessions(),
                        gymId: command.GymId);

        var addRoomResult = gym.AddRoom(room);

        if (addRoomResult.IsError)
        {
            return addRoomResult.Errors;
        }

        await _gymsRepository.UpdateAsync(gym);

        return room;
    }
}