using System.Data.Common;
using GoGym.Domain.SessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
                .ValueGeneratedNever();

        builder.Property(s => s.TrainerId);

        builder.OwnsMany<Reservation>("_reservations", rb =>
        {
            rb.ToTable("SessionReservations");
            
            rb.HasKey(r => r.Id);

            rb.Property(r => r.Id)
                .ValueGeneratedNever();

            rb.WithOwner().HasForeignKey("SessionId");

            rb.Property(r => r.ParticipantId);
        });

        builder.Property(s => s.MaxParticipants);

        builder.Property(s => s.Date);

        builder.OwnsOne(s => s.Time, tb =>
        {
            tb.Property(t => t.Start).HasColumnName("StartTime");
            tb.Property(t => t.End).HasColumnName("EndTime");
        });

        builder.Property(s => s.Name);

        builder.Property(s => s.Description);

        builder.Property(s => s.RoomId);

    }
}