using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class RemittanceLineConfiguration : IEntityTypeConfiguration<RemittanceLine>
{
    public void Configure(EntityTypeBuilder<RemittanceLine> builder)
    {
        builder.ToTable("RemittanceLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Amount).HasPrecision(18, 2);

        builder.HasIndex(l => new { l.RemittanceId, l.DocumentId }).IsUnique();
    }
}
