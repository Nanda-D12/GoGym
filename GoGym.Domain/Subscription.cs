using System.Diagnostics;
using ErrorOr;

namespace GoGym.Domain;

public class Subscription
{
    private readonly Guid _id;
    private readonly List<Guid> _gymIds = new();
    private readonly SubscriptionType _subscriptionType;
    private readonly int _maxGyms;
    private readonly Guid _adminId;

    public Subscription(SubscriptionType subscriptionType, Guid adminId, Guid? id = null)
    {
        _subscriptionType = subscriptionType;
        _adminId = adminId;
        _maxGyms = subscriptionType.MaxGyms;
        _id = id ?? Guid.NewGuid();
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
}