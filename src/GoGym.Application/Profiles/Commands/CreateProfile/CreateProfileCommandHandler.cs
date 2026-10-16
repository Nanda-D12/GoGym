using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ErrorOr;
using GoGym.Application.Common.Interfaces;
using GoGym.Domain.AdminAggregate;
using GoGym.Domain.ParticipantAggregate;
using GoGym.Domain.Profiles;
using GoGym.Domain.TrainerAggregate;
using MediatR;

namespace GoGym.Application.Profiles.Commands.CreateProfile;

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ErrorOr<Guid>>
{
    private readonly IAdminsRepository _adminRepository;
    private readonly ITrainersRepository _trainerRepository;
    private readonly IParticipantsRepository _participantRepository;

    public CreateProfileCommandHandler(IAdminsRepository adminRepository, ITrainersRepository trainerRepository, IParticipantsRepository partcipantRepository)
    {
        _adminRepository = adminRepository;
        _trainerRepository = trainerRepository;
        _participantRepository = partcipantRepository;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        return command.ProfileType.Name switch
        {
            nameof(ProfileType.Admin) => await CreateAdminAsync(command.UserId),
            nameof(ProfileType.Trainer) => await CreateTrainerAsync(command.UserId),
            nameof(ProfileType.Participant) => await CreateParticipantAsync(command.UserId),
        };
    }

    private async Task<ErrorOr<Guid>> CreateParticipantAsync(Guid userId)
    {
        if(await _participantRepository.GetProfileByUserIdAsync(userId) is not null)
        {
            return Error.Conflict(description: "User already has a participant profile.");
        }
        
        var participant = new Participant(userId);

        await _participantRepository.AddParticipantAsync(participant);
        return participant.Id;
    }

    private async Task<ErrorOr<Guid>> CreateTrainerAsync(Guid userId)
    {
        if(await _trainerRepository.GetProfileByUserIdAsync(userId) is not null)
        {
            return Error.Conflict(description: "User already has a trainer profile.");
        }
        
        var trainer = new Trainer(userId);

        await _trainerRepository.AddTrainerAsync(trainer);
        return trainer.Id;
    }

    private async Task<ErrorOr<Guid>> CreateAdminAsync(Guid userId)
    {
        if(await _adminRepository.GetProfileByUserIdAsyc(userId) is not null)
        {
            return Error.Conflict(description: "User already has an admin profile.");
        }

        var admin = new Admin(userId);
        await _adminRepository.AddAdminAsync(admin);
        
        return admin.Id;
    }
}