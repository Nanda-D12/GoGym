using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Application.Profiles.Common;
using MediatR;

namespace GoGym.Application.Profiles.Queries.ListProfiles;

public class ListProfilesQueryHandler : IRequestHandler<ListProfilesQuery, ErrorOr<List<Profile>>>
{
    private readonly IAdminsRepository _adminsRepository;
    private readonly IParticipantsRepository _participantsRepository;
    private readonly ITrainersRepository _trainersRepository;

    public ListProfilesQueryHandler(IAdminsRepository adminsRepository, IParticipantsRepository participantsRepository, ITrainersRepository trainersRepository)
    {
        _adminsRepository = adminsRepository;
        _participantsRepository = participantsRepository;
        _trainersRepository = trainersRepository;
    }
    public async Task<ErrorOr<List<Profile>>> Handle(ListProfilesQuery query, CancellationToken cancellationToken)
    {
        var admin = await _adminsRepository.GetProfileByUserIdAsyc(query.UserId);
        var trainer = await _trainersRepository.GetProfileByUserIdAsync(query.UserId);
        var participant = await _participantsRepository.GetProfileByUserIdAsync(query.UserId);

        var profiles = new[]
        {
            admin,
            trainer,
            participant
        }
        .Where(profile => profile is not null)
        .ToList();

        if (profiles.Count == 0)
        {
            return Error.NotFound(description: "User not found.");
        }

        return profiles!;
    }
}