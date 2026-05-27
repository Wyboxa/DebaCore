using Debales.Domain.Core.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations;

internal sealed class SystemModuleConfiguration : IEntityTypeConfiguration<SystemModule>
{
    public void Configure(EntityTypeBuilder<SystemModule> builder)
    {
        builder.ToTable("SystemModules");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(m => m.Name).IsUnique();

        builder.Property(m => m.Version)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.IsEnabled).IsRequired();
        builder.Property(m => m.DependenciesJson).HasMaxLength(1000);
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.CreatedBy).HasMaxLength(100);
        builder.Property(m => m.UpdatedBy).HasMaxLength(100);
        builder.Property(m => m.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
