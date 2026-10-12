using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.SessionAggregate;
using MediatR;

namespace GoGym.Application.Participants.Queries;

public class ListParticipantSessionsQueryHandler : IRequestHandler<ListParticipantSessionsQuery, ErrorOr<List<Session>>>
{
    private readonly IParticipantsRepository _participantsRepository;
    private readonly ISessionsRepository _sessionsRepository;

    public ListParticipantSessionsQueryHandler(IParticipantsRepository participantsRepository, ISessionsRepository sessionsRepository)
    {
        _participantsRepository = participantsRepository;
        _sessionsRepository = sessionsRepository;
    }

    public async Task<ErrorOr<List<Session>>> Handle(ListParticipantSessionsQuery query, CancellationToken cancellationToken)
    {
        var participant = await _participantsRepository.GetByIdAsync(query.ParticipantId);

        if (participant is null)
        {
            return Error.NotFound(description: "Participant not found.");
        }

        return await _sessionsRepository.ListByIdsAsync(participant.SessionIds, query.StartDateTime, query.EndDateTime);
    }
}