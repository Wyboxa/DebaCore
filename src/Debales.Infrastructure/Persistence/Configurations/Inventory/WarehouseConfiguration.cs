using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Inventory;

internal sealed class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Code).HasMaxLength(20).IsRequired();
        builder.Property(w => w.Name).HasMaxLength(100).IsRequired();
        builder.Property(w => w.Description).HasMaxLength(500);

        builder.HasIndex(w => w.Code).IsUnique();

        builder.HasMany(w => w.Locations)
            .WithOne(l => l.Warehouse)
            .HasForeignKey(l => l.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(w => !w.IsDeleted);
    }
}
