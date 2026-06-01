using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class SupplierPaymentConfiguration : IEntityTypeConfiguration<SupplierPayment>
{
    public void Configure(EntityTypeBuilder<SupplierPayment> builder)
    {
        builder.ToTable("SupplierPayments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Number).HasMaxLength(30).IsRequired();
        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.Reference).HasMaxLength(100);
        builder.Property(p => p.Notes).HasMaxLength(500);

        builder.HasIndex(p => p.Number).IsUnique();
        builder.HasIndex(p => p.SupplierId);

        builder.HasOne(p => p.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Payable)
            .WithMany()
            .HasForeignKey(p => p.PayableId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
