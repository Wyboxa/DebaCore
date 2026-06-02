using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class AccountingEntryLineConfiguration : IEntityTypeConfiguration<AccountingEntryLine>
{
    public void Configure(EntityTypeBuilder<AccountingEntryLine> builder)
    {
        builder.ToTable("AccountingEntryLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.AccountingEntryId).IsRequired();
        builder.Property(l => l.SortOrder).IsRequired();
        builder.Property(l => l.AccountId).IsRequired();
        builder.Property(l => l.AccountCode).IsRequired().HasMaxLength(20);
        builder.Property(l => l.Description).IsRequired().HasMaxLength(500);
        builder.Property(l => l.Debit).HasPrecision(18, 2);
        builder.Property(l => l.Credit).HasPrecision(18, 2);
        builder.Property(l => l.ThirdPartyType).HasMaxLength(20);
        builder.Property(l => l.CreatedAt).IsRequired();

        builder.HasOne<Account>()
               .WithMany()
               .HasForeignKey(l => l.AccountId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
