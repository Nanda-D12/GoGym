using ErrorOr;
using GoGym.Domain.RoomAggregate;
using MediatR;

namespace GoGym.Application.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(
    Guid GymId,
    string Name
) : IRequest<ErrorOr<Room>>;