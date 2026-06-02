using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class AccountingEntryConfiguration : IEntityTypeConfiguration<AccountingEntry>
{
    public void Configure(EntityTypeBuilder<AccountingEntry> builder)
    {
        builder.ToTable("AccountingEntries");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Number).IsRequired().HasMaxLength(30);
        builder.HasIndex(e => e.Number).IsUnique();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        builder.Property(e => e.JournalId).IsRequired();
        builder.Property(e => e.FiscalPeriodId).IsRequired();
        builder.Property(e => e.Status).IsRequired();
        builder.Property(e => e.SourceType).HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.UpdatedBy).HasMaxLength(100);
        builder.Property(e => e.DeletedBy).HasMaxLength(100);

        builder.HasOne(e => e.Journal)
               .WithMany()
               .HasForeignKey(e => e.JournalId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.FiscalPeriod)
               .WithMany()
               .HasForeignKey(e => e.FiscalPeriodId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Lines)
               .WithOne()
               .HasForeignKey(l => l.AccountingEntryId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
