using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Sessions.Queries.GetSession;

public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, ErrorOr<Session>>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly ISessionsRepository _sessionsRepository;

    public GetSessionQueryHandler(IRoomsRepository roomRepository, ISessionsRepository sessionsRepository)
    {
        _roomsRepository = roomRepository;
        _sessionsRepository = sessionsRepository;
    }

    public async Task<ErrorOr<Session>> Handle(GetSessionQuery query, CancellationToken cancellationToken)
    {
        var room = await _roomsRepository.GetByIdAsync(query.RoomId);

        if (room is null)
        {
            return Error.NotFound(description: "Room not found.");
        }

        if (!room.HasSession(query.SessionId))
        {
            return Error.NotFound(description: "Session not found.");
        }

        var session = await _sessionsRepository.GetByIdAsync(query.SessionId);

        if (session is null)
        {
            return Error.NotFound(description: "Session not found.");
        }

        return session;
    }
}