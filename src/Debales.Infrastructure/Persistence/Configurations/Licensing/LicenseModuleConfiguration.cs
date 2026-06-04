using Debales.Domain.Licensing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Licensing;

internal sealed class LicenseModuleConfiguration : IEntityTypeConfiguration<LicenseModule>
{
    public void Configure(EntityTypeBuilder<LicenseModule> builder)
    {
        builder.ToTable("LicenseModules");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.LicenseId).IsRequired();
        builder.Property(m => m.ModuleCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(m => new { m.LicenseId, m.ModuleCode }).IsUnique();
        builder.Property(m => m.GrantedAt).IsRequired();
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.CreatedBy).HasMaxLength(100);
        builder.Property(m => m.UpdatedBy).HasMaxLength(100);
    }
}
