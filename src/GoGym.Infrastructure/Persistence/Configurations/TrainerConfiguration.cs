using GoGym.Domain;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.TrainerAggregate;
using GoGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
                .ValueGeneratedNever();

        builder.Property(t => t.UserId);

        builder.Property<List<Guid>>("_sessionIds")
                .HasColumnName("SessiondIds")
                .HasListOfIdsConverter();

        builder.OwnsOne<Schedule>("_schedule", sb =>
        {
            sb.Property<Dictionary<DateOnly, List<TimeRange>>>("_caledar")
                .HasColumnName("ScheduleCalendar")
                .HasValueJsonConverter();

            sb.Property(s => s.Id)
                .HasColumnName("ScheduleId");
        });
    }
}