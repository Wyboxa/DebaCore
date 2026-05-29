using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class PurchaseDeliveryNoteConfiguration : IEntityTypeConfiguration<PurchaseDeliveryNote>
{
    public void Configure(EntityTypeBuilder<PurchaseDeliveryNote> builder)
    {
        builder.ToTable("PurchaseDeliveryNotes");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Number).HasMaxLength(30).IsRequired();
        builder.Property(n => n.Status).IsRequired();
        builder.Property(n => n.Notes).HasMaxLength(1000);

        builder.HasIndex(n => n.Number).IsUnique();
        builder.HasIndex(n => n.SupplierId);
        builder.HasIndex(n => n.PurchaseOrderId);

        builder.HasOne(n => n.Supplier)
            .WithMany()
            .HasForeignKey(n => n.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.PurchaseOrder)
            .WithMany()
            .HasForeignKey(n => n.PurchaseOrderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.Lines)
            .WithOne()
            .HasForeignKey(l => l.PurchaseDeliveryNoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}
