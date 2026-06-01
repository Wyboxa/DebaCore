using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Inventory;

internal sealed class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Number).HasMaxLength(30).IsRequired();
        builder.Property(m => m.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(m => m.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(m => m.Quantity).HasPrecision(18, 4);
        builder.Property(m => m.Reference).HasMaxLength(100);
        builder.Property(m => m.Notes).HasMaxLength(500);

        builder.HasIndex(m => m.Number).IsUnique();
        builder.HasIndex(m => m.ItemId);
        builder.HasIndex(m => m.WarehouseId);
        builder.HasIndex(m => m.Date);

        builder.HasOne(m => m.Item)
            .WithMany()
            .HasForeignKey(m => m.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Warehouse)
            .WithMany()
            .HasForeignKey(m => m.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Location)
            .WithMany()
            .HasForeignKey(m => m.LocationId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasQueryFilter(m => !m.IsDeleted);
    }
}
