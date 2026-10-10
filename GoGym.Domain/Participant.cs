namespace GoGym.Domain;
public class Participant
{
    private readonly Guid _userId;
    private readonly List<Guid> _sessionIds;
    public Guid Id { get; }

    public Participant(Guid userId, Guid id = default)
    {
        _userId = userId;
        Id = id;
    }

}