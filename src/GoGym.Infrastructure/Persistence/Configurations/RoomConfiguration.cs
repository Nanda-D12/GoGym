using GoGym.Domain;
using GoGym.Domain.Common.ValueObjects;
using GoGym.Domain.RoomAggregate;
using GoGym.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
                .ValueGeneratedNever();

        builder.Property(r => r.GymId);

        builder.Property("_maxDailySessions");

        builder.OwnsOne<Schedule>("_schedule", sb =>
        {
            sb.Property<Dictionary<DateOnly, List<TimeRange>>>("_calendar")
                .HasColumnName("ScheduleCalendar")
                .HasValueJsonConverter();

            sb.Property(s => s.Id)
                .HasColumnName("ScheduleId");
        });

        builder.Property(r => r.Name);
    }
}