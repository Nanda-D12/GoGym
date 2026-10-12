using GoGym.Domain;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.ParticipantAggregate;
using GoGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
                .ValueGeneratedNever();

        builder.Property("_userId")
                .HasColumnName("UserId");

        builder.Property<List<Guid>>(p => p.SessionIds)
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