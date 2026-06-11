using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Inventory;

internal sealed class InventoryCountLineConfiguration : IEntityTypeConfiguration<InventoryCountLine>
{
    public void Configure(EntityTypeBuilder<InventoryCountLine> builder)
    {
        builder.ToTable("InventoryCountLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.SystemQuantity).HasPrecision(18, 4);
        builder.Property(l => l.CountedQuantity).HasPrecision(18, 4);

        builder.Ignore(l => l.Difference);
        builder.Ignore(l => l.IsCounted);

        builder.HasIndex(l => l.InventoryCountId);
        builder.HasIndex(l => l.ItemId);

        builder.HasOne(l => l.Item)
            .WithMany()
            .HasForeignKey(l => l.ItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
