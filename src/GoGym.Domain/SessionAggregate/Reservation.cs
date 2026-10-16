using GoGym.Domain.Common;

namespace GoGym.Domain.SessionAggregate;

public class Reservation : Entity
{
    public Guid ParticipantId { get; }
    public Reservation(Guid participantId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        ParticipantId = participantId;
    }

    private Reservation() {}
}