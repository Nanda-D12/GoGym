using GoGym.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class OutboxMessaageConnfiguration : IEntityTypeConfiguration<OutboxMessaage>
{
    public void Configure(EntityTypeBuilder<OutboxMessaage> builder)
    {
        builder.ToTable("OutboxMessages");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Type).HasMaxLength(255);
        builder.Property(o => o.Content).HasColumnType("nvarchar(max)");
    }
}