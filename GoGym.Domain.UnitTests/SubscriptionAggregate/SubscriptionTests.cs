using FluentAssertions;
using GoGym.Domain.SubscriptionAggregate;
using GoGym.Domain.UnitTests.TestUtils.Gyms;
using GoGym.Domain.UnitTests.TestUtils.Subscriptions;

namespace GoGym.Domain.UnitTests.SubscriptionAggregate;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // arrange
        var subscription = SubscriptionFactory.CreateSubscription();
        var gym1 = GymFactory.CreateGym(id: Guid.NewGuid());
        var gym2 = GymFactory.CreateGym(id: Guid.NewGuid());

        // act
        var addGym1Result = subscription.AddGym(gym1);
        var addGym2Result = subscription.AddGym(gym2);

        // assert
        // last gym add should fail
        addGym1Result.IsError.Should().BeFalse();
        addGym2Result.IsError.Should().BeTrue();

        addGym2Result.FirstError.Should().Be(SubscriptionErrors.CannotAddGymMoreThanSubscriptionAllows);
    }
}