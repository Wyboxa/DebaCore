using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class PurchaseCreditNoteConfiguration : IEntityTypeConfiguration<PurchaseCreditNote>
{
    public void Configure(EntityTypeBuilder<PurchaseCreditNote> builder)
    {
        builder.ToTable("PurchaseCreditNotes");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Number).HasMaxLength(30).IsRequired();
        builder.Property(n => n.Reason).HasMaxLength(500);

        builder.HasIndex(n => n.Number).IsUnique();
        builder.HasIndex(n => n.SupplierId);

        builder.HasOne(n => n.Supplier)
            .WithMany()
            .HasForeignKey(n => n.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.OriginalInvoice)
            .WithMany()
            .HasForeignKey(n => n.OriginalInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.Lines)
            .WithOne()
            .HasForeignKey(l => l.PurchaseCreditNoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}
