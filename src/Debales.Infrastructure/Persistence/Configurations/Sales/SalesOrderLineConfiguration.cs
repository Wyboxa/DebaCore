using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Description).HasMaxLength(500);
        builder.Property(l => l.Quantity).HasColumnType("decimal(18,4)").IsRequired();
        builder.Property(l => l.UnitPrice).HasColumnType("decimal(18,4)").IsRequired();
        builder.Property(l => l.TaxRate).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(l => l.DeliveredQuantity).HasColumnType("decimal(18,4)").IsRequired();

        builder.Ignore(l => l.LineSubtotal);
        builder.Ignore(l => l.LineTaxAmount);
        builder.Ignore(l => l.LineTotal);
        builder.Ignore(l => l.PendingQuantity);
    }
}
