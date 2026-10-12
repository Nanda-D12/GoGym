using ErrorOr;
using GoGym.Domain.RoomAggregate;
using MediatR;

namespace GoGym.Application.Rooms.Queries.GetRoom;

public record GetRoomQuery(
    Guid GymId,
    Guid RoomId
) : IRequest<ErrorOr<Room>>;