using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class ReceivableConfiguration : IEntityTypeConfiguration<Receivable>
{
    public void Configure(EntityTypeBuilder<Receivable> builder)
    {
        builder.ToTable("Receivables");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Number).HasMaxLength(30).IsRequired();
        builder.Property(r => r.OriginalAmount).HasPrecision(18, 2);
        builder.Property(r => r.PaidAmount).HasPrecision(18, 2);

        builder.HasIndex(r => r.Number).IsUnique();
        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.DueDate);

        builder.HasOne(r => r.SalesInvoice)
            .WithMany()
            .HasForeignKey(r => r.SalesInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Customer)
            .WithMany()
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
