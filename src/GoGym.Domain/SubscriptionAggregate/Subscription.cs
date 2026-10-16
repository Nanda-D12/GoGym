using ErrorOr;
using GoGym.Domain.Common;
using GoGym.Domain.GymAggregate;

namespace GoGym.Domain.SubscriptionAggregate;

public class Subscription : AggregateRoot
{
    private readonly List<Guid> _gymIds = new();
    private readonly int _maxGyms;
    private readonly Guid _adminId;

    public SubscriptionType SubscriptionType { get; }

    public Subscription(SubscriptionType subscriptionType, Guid adminId, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        SubscriptionType = subscriptionType;
        _adminId = adminId;
        _maxGyms = subscriptionType.MaxGyms;
    }

    public int GetMaxRooms()
    {
        return SubscriptionType.MaxRooms;
    }

    public int GetMaxDailySessions()
    {
        return SubscriptionType.MaxDailySessions;
    }

    public bool HasGym(Guid gymId)
    {
        return _gymIds.Contains(gymId);
    }
    
    public ErrorOr<Success> AddGym(Gym gym)
    {
        if (_gymIds.Count >= _maxGyms)
        {
            return SubscriptionErrors.CannotAddGymMoreThanSubscriptionAllows;
        }
        if (_gymIds.Contains(gym.Id))
        {
            return SubscriptionErrors.CannotAddSameGym;
        }

        _gymIds.Add(gym.Id);

        return Result.Success;
    }

    public Subscription() {}
}