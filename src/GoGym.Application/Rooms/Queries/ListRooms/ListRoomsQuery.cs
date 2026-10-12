using ErrorOr;
using GoGym.Domain.RoomAggregate;
using MediatR;

namespace GoGym.Application.Rooms.Queries.ListRooms;

public record ListRoomsQuery (
    Guid GymId
) : IRequest<ErrorOr<List<Room>>>;