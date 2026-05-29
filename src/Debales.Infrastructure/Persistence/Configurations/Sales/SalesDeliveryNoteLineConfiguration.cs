using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class SalesDeliveryNoteLineConfiguration : IEntityTypeConfiguration<SalesDeliveryNoteLine>
{
    public void Configure(EntityTypeBuilder<SalesDeliveryNoteLine> builder)
    {
        builder.ToTable("SalesDeliveryNoteLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Description).HasMaxLength(500);
        builder.Property(l => l.Quantity).HasColumnType("decimal(18,4)").IsRequired();
    }
}
