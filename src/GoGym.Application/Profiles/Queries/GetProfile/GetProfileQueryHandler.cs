using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Application.Profiles.Common;
using GoGym.Domain.Profiles;
using MediatR;

namespace GoGym.Application.Profiles.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ErrorOr<Profile?>>
{
    private readonly IAdminsRepository _adminsRepository;
    private readonly IParticipantsRepository _participantsRepository;
    private readonly ITrainersRepository _trainersRepository;

    public GetProfileQueryHandler(IAdminsRepository adminsRepository, IParticipantsRepository participantsRepository, ITrainersRepository trainersRepository)
    {
        _adminsRepository = adminsRepository;
        _participantsRepository = participantsRepository;
        _trainersRepository = trainersRepository;
    }

    public async Task<ErrorOr<Profile?>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return request.ProfileType.Name switch
        {
            nameof(ProfileType.Admin) => await _adminsRepository.GetProfileByUserIdAsyc(request.UserId),
            nameof(ProfileType.Trainer) => await _trainersRepository.GetProfileByUserIdAsync(request.UserId),
            nameof(ProfileType.Participant) => await _participantsRepository.GetProfileByUserIdAsync(request.UserId),
            _ => Error.Unexpected(description: "Unexpected profile type.")
        };
    }
}