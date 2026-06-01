using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class CustomerPaymentConfiguration : IEntityTypeConfiguration<CustomerPayment>
{
    public void Configure(EntityTypeBuilder<CustomerPayment> builder)
    {
        builder.ToTable("CustomerPayments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Number).HasMaxLength(30).IsRequired();
        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.Reference).HasMaxLength(100);
        builder.Property(p => p.Notes).HasMaxLength(500);

        builder.HasIndex(p => p.Number).IsUnique();
        builder.HasIndex(p => p.CustomerId);

        builder.HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Receivable)
            .WithMany()
            .HasForeignKey(p => p.ReceivableId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
