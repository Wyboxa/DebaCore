using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Sales;

internal sealed class PaymentTermLineConfiguration : IEntityTypeConfiguration<PaymentTermLine>
{
    public void Configure(EntityTypeBuilder<PaymentTermLine> builder)
    {
        builder.ToTable("PaymentTermLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.DueDays).IsRequired();
        builder.Property(l => l.Percentage).IsRequired().HasPrecision(5, 2);
        builder.Property(l => l.SortOrder).IsRequired();
        builder.Property(l => l.CreatedAt).IsRequired();
    }
}
