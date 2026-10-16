using ErrorOr;
using GoGym.Application.Profiles.Common;
using MediatR;

namespace GoGym.Application.Profiles.Queries.ListProfiles;

public record ListProfilesQuery (Guid UserId) : IRequest<ErrorOr<List<Profile>>>;