using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class AccountingJournalConfiguration : IEntityTypeConfiguration<AccountingJournal>
{
    public void Configure(EntityTypeBuilder<AccountingJournal> builder)
    {
        builder.ToTable("AccountingJournals");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Code).IsRequired().HasMaxLength(10);
        builder.HasIndex(j => j.Code).IsUnique();
        builder.Property(j => j.Name).IsRequired().HasMaxLength(100);
        builder.Property(j => j.IsActive).IsRequired();
        builder.Property(j => j.CreatedAt).IsRequired();
        builder.Property(j => j.CreatedBy).HasMaxLength(100);
        builder.Property(j => j.UpdatedBy).HasMaxLength(100);
        builder.Property(j => j.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(j => !j.IsDeleted);

        builder.HasData(
            AccountingJournal.ForSeed(AccountingSeeds.Journal_VTA, "VTA", "Diario de Ventas"),
            AccountingJournal.ForSeed(AccountingSeeds.Journal_CPR, "CPR", "Diario de Compras"),
            AccountingJournal.ForSeed(AccountingSeeds.Journal_BCO, "BCO", "Diario de Banco"),
            AccountingJournal.ForSeed(AccountingSeeds.Journal_CAJ, "CAJ", "Diario de Caja")
        );
    }
}
