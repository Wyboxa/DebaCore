using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Code).IsRequired().HasMaxLength(20);
        builder.HasIndex(a => a.Code).IsUnique();
        builder.Property(a => a.Name).IsRequired().HasMaxLength(200);
        builder.Property(a => a.Type).IsRequired();
        builder.Property(a => a.IsPostable).IsRequired();
        builder.Property(a => a.IsActive).IsRequired();
        builder.Property(a => a.ParentCode).HasMaxLength(20);
        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.CreatedBy).HasMaxLength(100);
        builder.Property(a => a.UpdatedBy).HasMaxLength(100);
        builder.Property(a => a.DeletedBy).HasMaxLength(100);

        builder.HasQueryFilter(a => !a.IsDeleted);

        builder.HasData(
            Account.ForSeed(AccountingSeeds.Account_300, "300", "Mercaderías", AccountType.Asset, true, null),
            Account.ForSeed(AccountingSeeds.Account_400, "400", "Proveedores", AccountType.Liability, false, null),
            Account.ForSeed(AccountingSeeds.Account_430, "430", "Clientes", AccountType.Asset, false, null),
            Account.ForSeed(AccountingSeeds.Account_472, "472", "HP IVA soportado", AccountType.Asset, true, null),
            Account.ForSeed(AccountingSeeds.Account_475, "475", "HP acreedora por conceptos fiscales", AccountType.Liability, true, null),
            Account.ForSeed(AccountingSeeds.Account_477, "477", "HP IVA repercutido", AccountType.Liability, true, null),
            Account.ForSeed(AccountingSeeds.Account_570, "570", "Caja", AccountType.Asset, true, null),
            Account.ForSeed(AccountingSeeds.Account_572, "572", "Bancos c/c", AccountType.Asset, true, null),
            Account.ForSeed(AccountingSeeds.Account_600, "600", "Compras de mercaderías", AccountType.Expense, true, null),
            Account.ForSeed(AccountingSeeds.Account_621, "621", "Arrendamientos y cánones", AccountType.Expense, true, null),
            Account.ForSeed(AccountingSeeds.Account_628, "628", "Suministros", AccountType.Expense, true, null),
            Account.ForSeed(AccountingSeeds.Account_640, "640", "Sueldos y salarios", AccountType.Expense, true, null),
            Account.ForSeed(AccountingSeeds.Account_700, "700", "Ventas de mercaderías", AccountType.Revenue, true, null),
            Account.ForSeed(AccountingSeeds.Account_705, "705", "Prestaciones de servicios", AccountType.Revenue, true, null)
        );
    }
}
