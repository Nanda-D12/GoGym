using ErrorOr;
using GoGym.Domain.Profiles;
using MediatR;

namespace GoGym.Application.Profiles.Commands.CreateProfile;

public record CreateProfileCommand (
    ProfileType ProfileType,
    Guid UserId
) : IRequest<ErrorOr<Guid>>;