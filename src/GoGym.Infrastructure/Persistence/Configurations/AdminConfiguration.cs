using GoGym.Domain.AdminAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoGym.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
                .ValueGeneratedNever();

        builder.Property("_userId")
                .HasColumnName("UserId");

        builder.Property(a => a.SubscriptionId);
    }
}