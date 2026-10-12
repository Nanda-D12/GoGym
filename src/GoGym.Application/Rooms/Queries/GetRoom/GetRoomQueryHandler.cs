using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.RoomAggregate;
using MediatR;

namespace GoGym.Application.Rooms.Queries.GetRoom;

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, ErrorOr<Room>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public GetRoomQueryHandler(IGymsRepository gymsRepository, IRoomsRepository roomsRepository)
    {
        _gymsRepository = gymsRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<Room>> Handle(GetRoomQuery query, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(query.GymId);

        if ( gym is null)
        {
            return Error.NotFound(description: "Gym not found.");
        }

        if (!gym.HasRoom(query.RoomId))
        {
            return Error.NotFound(description: "Room not found.");
        }

        var room = await _roomsRepository.GetByIdAsync(query.RoomId);

        if ( room is null)
        {
            return Error.NotFound(description: "Room not found.");
        }

        return room;
    }
}