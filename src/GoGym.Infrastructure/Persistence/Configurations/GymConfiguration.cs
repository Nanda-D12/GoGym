using GoGym.Infrastructure.Persistence.Converters;
using GoGym.Domain.GymAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class GymConfiguration : IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
                .ValueGeneratedNever();

        builder.Property(g => g.Name);

        builder.Property(g => g.SubscriptionId);

        builder.Property("_maxRooms")
                .HasColumnName("MaxRooms");

        builder.Property<List<Guid>>("_trainerIds")
                .HasColumnName("TrainerIds")
                .HasListOfIdsConverter();

        builder.Property<List<Guid>>(g => g.RoomIds)
                .HasListOfIdsConverter();

    }
}