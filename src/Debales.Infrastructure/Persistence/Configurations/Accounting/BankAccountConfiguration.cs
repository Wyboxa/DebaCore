using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("BankAccounts");
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name).IsRequired().HasMaxLength(150);
        builder.Property(b => b.BankName).HasMaxLength(150);
        builder.Property(b => b.Iban).HasMaxLength(34);
        builder.Property(b => b.Bic).HasMaxLength(11);
        builder.Property(b => b.CurrencyCode).HasMaxLength(3).HasDefaultValue("EUR");
        builder.Property(b => b.IsActive).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired();
        builder.Property(b => b.CreatedBy).HasMaxLength(100);
        builder.Property(b => b.UpdatedBy).HasMaxLength(100);
        builder.Property(b => b.DeletedBy).HasMaxLength(100);

        builder.HasOne<Account>().WithMany()
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasIndex(b => b.Iban).IsUnique().HasFilter("[Iban] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasQueryFilter(b => !b.IsDeleted);
    }
}
