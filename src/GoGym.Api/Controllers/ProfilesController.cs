using GoGym.Application.Profiles.Commands.CreateProfile;
using GoGym.Application.Profiles.Queries.GetProfile;
using GoGym.Application.Profiles.Queries.ListProfiles;
using GoGym.Contracts.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoGym.Api.Controllers;

[Route("users/{userId:guid}/profiles")]
public class ProfilesController : ApiController
{
    private readonly ISender _sender;

    public ProfilesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile(CreateProfileRequest request, Guid userId)
    {
        if(!Domain.Profiles.ProfileType.TryFromName(request.ProfileType.ToString(), out var profileType))
        {
            return Problem("Invalid profile type", statusCode: StatusCodes.Status400BadRequest);
        }

        var command = new CreateProfileCommand(profileType, userId);

        var createProfileResult = await _sender.Send(command);

        return createProfileResult.Match(
            id => CreatedAtAction(
                nameof(GetProfile),
                new { userId, profileType = request.ProfileType.ToString()},
                new ProfileResponse(id, request.ProfileType)
            ),
            Problem
        );
    }

    [HttpGet("{profileType}")]
    public async Task<IActionResult> GetProfile(Guid userId, string profileType)
    {
        if(!Domain.Profiles.ProfileType.TryFromName(profileType, out var profile))
        {
            return Problem("Invalid profile type", statusCode: StatusCodes.Status400BadRequest);
        }
        var query = new GetProfileQuery(userId, profile);

        var getProfileResult = await _sender.Send(query);

        return getProfileResult.Match(
            profile => profile is null 
                        ? Problem(statusCode: StatusCodes.Status404NotFound)
                        :  Ok(new ProfileResponse(profile.Id, ToDto(profile.ProfileType))),
            Problem
        );
    }

    [HttpGet]
    public async Task<IActionResult> ListProfiles(Guid userId)
    {
        var query = new ListProfilesQuery(userId);

        var listProfilesResult = await _sender.Send(query);

        return listProfilesResult.Match(
            profiles => Ok(profiles.ConvertAll(
                profile => new ProfileResponse(profile.Id, ToDto(profile.ProfileType))
            )),
            Problem
        );
    }

    public static Contracts.Profiles.ProfileType ToDto(Domain.Profiles.ProfileType profileType)
    {
        return profileType.Name switch
        {
            nameof(Domain.Profiles.ProfileType.Admin) => ProfileType.Admin,
            nameof(Domain.Profiles.ProfileType.Trainer) => ProfileType.Trainer,
            nameof(Domain.Profiles.ProfileType.Participant) => ProfileType.Participant,
            _ => throw new InvalidOperationException()
        };
    }
}