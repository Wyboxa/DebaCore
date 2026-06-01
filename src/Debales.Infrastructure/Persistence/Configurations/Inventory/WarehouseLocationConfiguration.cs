using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Inventory;

internal sealed class WarehouseLocationConfiguration : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        builder.ToTable("WarehouseLocations");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Code).HasMaxLength(20).IsRequired();
        builder.Property(l => l.Description).HasMaxLength(200);

        builder.HasIndex(l => new { l.WarehouseId, l.Code }).IsUnique();

        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}
