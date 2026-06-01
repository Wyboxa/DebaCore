using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesCreditNoteConfiguration : IEntityTypeConfiguration<SalesCreditNote>
{
    public void Configure(EntityTypeBuilder<SalesCreditNote> builder)
    {
        builder.ToTable("SalesCreditNotes");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Number).HasMaxLength(30).IsRequired();
        builder.Property(n => n.Reason).HasMaxLength(500);

        builder.HasIndex(n => n.Number).IsUnique();
        builder.HasIndex(n => n.CustomerId);

        builder.HasOne(n => n.Customer)
            .WithMany()
            .HasForeignKey(n => n.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.OriginalInvoice)
            .WithMany()
            .HasForeignKey(n => n.OriginalInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.Lines)
            .WithOne()
            .HasForeignKey(l => l.SalesCreditNoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(n => !n.IsDeleted);
    }
}
