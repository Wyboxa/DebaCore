using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesQuoteLineConfiguration : IEntityTypeConfiguration<SalesQuoteLine>
{
    public void Configure(EntityTypeBuilder<SalesQuoteLine> builder)
    {
        builder.ToTable("SalesQuoteLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Description).HasMaxLength(500);
        builder.Property(l => l.Quantity).HasColumnType("decimal(18,4)");
        builder.Property(l => l.UnitPrice).HasColumnType("decimal(18,4)");
        builder.Property(l => l.TaxRate).HasColumnType("decimal(5,2)");
    }
}
