using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class FiscalYearConfiguration : IEntityTypeConfiguration<FiscalYear>
{
    public void Configure(EntityTypeBuilder<FiscalYear> builder)
    {
        builder.ToTable("FiscalYears");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(f => f.Name).IsUnique().HasFilter("[Name] IS NOT NULL");
        builder.Property(f => f.StartDate).IsRequired();
        builder.Property(f => f.EndDate).IsRequired();
        builder.Property(f => f.Status).IsRequired();
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.CreatedBy).HasMaxLength(100);
        builder.Property(f => f.UpdatedBy).HasMaxLength(100);
        builder.Property(f => f.DeletedBy).HasMaxLength(100);

        builder.HasMany(f => f.Periods)
               .WithOne()
               .HasForeignKey(p => p.FiscalYearId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}
