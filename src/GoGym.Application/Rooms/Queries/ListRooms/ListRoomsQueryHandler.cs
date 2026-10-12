using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.RoomAggregate;
using MediatR;

namespace GoGym.Application.Rooms.Queries.ListRooms;

public class ListRoomsQueryHandler : IRequestHandler<ListRoomsQuery, ErrorOr<List<Room>>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IRoomsRepository _roomsRepository;

    public ListRoomsQueryHandler(IGymsRepository gymsRepository, IRoomsRepository roomsRepository)
    {
        _gymsRepository = gymsRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<ErrorOr<List<Room>>> Handle(ListRoomsQuery query, CancellationToken cancellationToken)
    {
        var gym = await _gymsRepository.GetByIdAsync(query.GymId);

        if ( gym is null )
        {
            return Error.NotFound(description: "Gym not found.");
        }

        var rooms = await _roomsRepository.ListByGymIdAsync(query.GymId);

        return rooms;
    }
}