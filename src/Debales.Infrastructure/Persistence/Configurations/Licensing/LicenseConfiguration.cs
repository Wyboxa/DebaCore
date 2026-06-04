using Debales.Domain.Licensing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Licensing;

internal sealed class LicenseConfiguration : IEntityTypeConfiguration<License>
{
    public void Configure(EntityTypeBuilder<License> builder)
    {
        builder.ToTable("Licenses");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.InstallationId).IsRequired().HasMaxLength(200);
        builder.HasIndex(l => l.InstallationId).IsUnique();
        builder.Property(l => l.LicenseeCompany).IsRequired().HasMaxLength(200);
        builder.Property(l => l.LicenseeEmail).IsRequired().HasMaxLength(150);
        builder.Property(l => l.LicenseKey).IsRequired().HasMaxLength(100);
        builder.HasIndex(l => l.LicenseKey).IsUnique();
        builder.Property(l => l.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(l => l.StartsAt).IsRequired();
        builder.Property(l => l.ExpiresAt).IsRequired();
        builder.Property(l => l.Notes).HasMaxLength(500);
        builder.Property(l => l.CreatedAt).IsRequired();
        builder.Property(l => l.CreatedBy).HasMaxLength(100);
        builder.Property(l => l.UpdatedBy).HasMaxLength(100);

        builder.HasOne(l => l.Plan)
            .WithMany()
            .HasForeignKey(l => l.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.Modules)
            .WithOne()
            .HasForeignKey(m => m.LicenseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
