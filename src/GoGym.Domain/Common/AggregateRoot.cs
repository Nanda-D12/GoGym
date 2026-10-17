
namespace GoGym.Domain.Common;

public abstract class AggregateRoot : Entity
{
    protected readonly List<IDomainEvent> _domainEvents = new();
    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents;
        _domainEvents.Clear();

        return copy;
    }
    
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot() : base() {}
}