using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class CashAccountConfiguration : IEntityTypeConfiguration<CashAccount>
{
    public void Configure(EntityTypeBuilder<CashAccount> builder)
    {
        builder.ToTable("CashAccounts");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Code).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Name).HasMaxLength(150).IsRequired();
        builder.Property(a => a.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(a => a.CurrentBalance).HasPrecision(18, 2);

        builder.HasIndex(a => a.Code).IsUnique();

        builder.HasOne(a => a.Account)
            .WithMany()
            .HasForeignKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}
