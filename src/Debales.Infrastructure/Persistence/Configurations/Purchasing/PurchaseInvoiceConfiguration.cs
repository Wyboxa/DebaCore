using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class PurchaseInvoiceConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
    {
        builder.ToTable("PurchaseInvoices");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Number).HasMaxLength(30).IsRequired();
        builder.Property(i => i.SupplierInvoiceNumber).HasMaxLength(50);
        builder.Property(i => i.Status).IsRequired();
        builder.Property(i => i.Notes).HasMaxLength(1000);

        builder.HasIndex(i => i.Number).IsUnique();
        builder.HasIndex(i => i.SupplierId);
        builder.HasIndex(i => i.Status);

        builder.HasOne(i => i.Supplier)
            .WithMany()
            .HasForeignKey(i => i.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.PurchaseDeliveryNote)
            .WithMany()
            .HasForeignKey(i => i.PurchaseDeliveryNoteId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(i => i.Lines)
            .WithOne()
            .HasForeignKey(l => l.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(i => !i.IsDeleted);
    }
}
