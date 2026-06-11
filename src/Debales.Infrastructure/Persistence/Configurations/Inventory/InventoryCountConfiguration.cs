using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Inventory;

internal sealed class InventoryCountConfiguration : IEntityTypeConfiguration<InventoryCount>
{
    public void Configure(EntityTypeBuilder<InventoryCount> builder)
    {
        builder.ToTable("InventoryCounts");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Number).HasMaxLength(30).IsRequired();
        builder.Property(c => c.Notes).HasMaxLength(500);

        builder.HasIndex(c => c.Number).IsUnique();
        builder.HasIndex(c => c.WarehouseId);
        builder.HasIndex(c => c.Date);

        builder.HasOne(c => c.Warehouse)
            .WithMany()
            .HasForeignKey(c => c.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Lines)
            .WithOne(l => l.InventoryCount)
            .HasForeignKey(l => l.InventoryCountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
