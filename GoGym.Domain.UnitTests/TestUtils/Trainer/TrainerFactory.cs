using GoGym.Domain.UnitTests.TestConstants;

namespace GoGym.Domain.UnitTests.TestUtils.Trainers;

public static class TrainerFactory
{
    public static Trainer CreateTrainer(
                            Guid? userId = null,
                            Guid? id = null
    )
    {
        var trainer = new Trainer(
                            userId: userId ?? Constants.User.Id,
                            id: id ?? Constants.Trainer.Id
        );

        return trainer;
    }
}