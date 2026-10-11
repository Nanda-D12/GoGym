using GoGym.Domain.GymAggregate;
using GoGym.Domain.UnitTests.TestConstants;

namespace GoGym.Domain.UnitTests.TestUtils.Gyms;

public static class GymFactory
{
    public static Gym CreateGym(int maxRooms = Constants.Subscription.MaxRoomsFreeTier,
                                Guid? id = null)
    {
        var gym = new Gym(maxRooms: maxRooms,
                            subscriptionId: Constants.Subscription.Id,
                            id: id ?? Constants.Gym.Id);
        return gym;
    }
}