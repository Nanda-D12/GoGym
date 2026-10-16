using ErrorOr;
using GoGym.Application.Profiles.Common;
using GoGym.Domain.Profiles;
using MediatR;

namespace GoGym.Application.Profiles.Queries.GetProfile;

public record GetProfileQuery (Guid UserId, ProfileType ProfileType)
    : IRequest<ErrorOr<Profile?>>;