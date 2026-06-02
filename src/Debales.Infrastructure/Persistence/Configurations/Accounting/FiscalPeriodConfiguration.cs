using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class FiscalPeriodConfiguration : IEntityTypeConfiguration<FiscalPeriod>
{
    public void Configure(EntityTypeBuilder<FiscalPeriod> builder)
    {
        builder.ToTable("FiscalPeriods");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FiscalYearId).IsRequired();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.EndDate).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.CreatedBy).HasMaxLength(100);
    }
}
