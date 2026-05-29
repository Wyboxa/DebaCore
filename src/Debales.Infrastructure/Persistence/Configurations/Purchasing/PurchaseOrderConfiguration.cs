using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Number).HasMaxLength(30).IsRequired();
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.Notes).HasMaxLength(1000);

        builder.HasIndex(o => o.Number).IsUnique();
        builder.HasIndex(o => o.SupplierId);
        builder.HasIndex(o => o.Status);

        builder.HasOne(o => o.Supplier)
            .WithMany()
            .HasForeignKey(o => o.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Lines)
            .WithOne()
            .HasForeignKey(l => l.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
