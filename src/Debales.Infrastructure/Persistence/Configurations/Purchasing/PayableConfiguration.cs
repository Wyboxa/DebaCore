using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class PayableConfiguration : IEntityTypeConfiguration<Payable>
{
    public void Configure(EntityTypeBuilder<Payable> builder)
    {
        builder.ToTable("Payables");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Number).HasMaxLength(30).IsRequired();
        builder.Property(p => p.OriginalAmount).HasPrecision(18, 2);
        builder.Property(p => p.PaidAmount).HasPrecision(18, 2);

        builder.HasIndex(p => p.Number).IsUnique();
        builder.HasIndex(p => p.SupplierId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.DueDate);

        builder.HasOne(p => p.PurchaseInvoice)
            .WithMany()
            .HasForeignKey(p => p.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
