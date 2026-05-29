using Debales.Domain.Purchasing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Purchasing;

internal sealed class PurchaseDeliveryNoteLineConfiguration : IEntityTypeConfiguration<PurchaseDeliveryNoteLine>
{
    public void Configure(EntityTypeBuilder<PurchaseDeliveryNoteLine> builder)
    {
        builder.ToTable("PurchaseDeliveryNoteLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.ItemCode).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Description).HasMaxLength(500);
        builder.Property(l => l.Quantity).HasColumnType("decimal(18,4)").IsRequired();
    }
}
