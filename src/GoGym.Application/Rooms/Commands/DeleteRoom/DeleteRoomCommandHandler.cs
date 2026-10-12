using System.ComponentModel;
using ErrorOr;
using GoGym.Application.Common.Interfaces;
using MediatR;

namespace GoGym.Application.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public DeleteRoomCommandHandler(IGymsRepository gymsRepository, IRoomsRepository roomsRepository)
    {
        _gymsRepository = gymsRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var room = await _roomsRepository.GetByIdAsync(command.RoomId);

        if (room is null)
        {
            return Error.NotFound(description: "Room not found.");
        }

        var removeGymResult = gym.RemoveRoom(room);

        if (removeGymResult.IsError)
        {
            return removeGymResult.Errors;
        }

        await _gymsRepository.UpdateAsync(gym);

        return Result.Deleted;
    }
}