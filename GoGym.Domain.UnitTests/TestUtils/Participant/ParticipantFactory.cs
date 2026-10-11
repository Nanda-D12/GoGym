using GoGym.Domain.ParticipantAggregate;
using GoGym.Domain.UnitTests.TestConstants;

namespace GoGym.Domain.UnitTests.TestUtils.Participants;

public static class ParticipantFactory
{
    public static Participant CreateParticipant(Guid? userId = null,
                                                Guid? id = null)
    {
        var pariticipant = new Participant(userId: userId ?? Constants.User.Id,
                                            id: id ?? Constants.Participant.Id);

        return pariticipant;
    }
}