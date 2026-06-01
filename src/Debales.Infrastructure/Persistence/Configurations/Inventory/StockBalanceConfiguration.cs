using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Inventory;

internal sealed class StockBalanceConfiguration : IEntityTypeConfiguration<StockBalance>
{
    public void Configure(EntityTypeBuilder<StockBalance> builder)
    {
        builder.ToTable("StockBalances");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Quantity).HasPrecision(18, 4);

        // Unique constraint: one balance row per (item, warehouse)
        builder.HasIndex(b => new { b.ItemId, b.WarehouseId }).IsUnique();

        builder.HasOne(b => b.Item)
            .WithMany()
            .HasForeignKey(b => b.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Warehouse)
            .WithMany()
            .HasForeignKey(b => b.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
