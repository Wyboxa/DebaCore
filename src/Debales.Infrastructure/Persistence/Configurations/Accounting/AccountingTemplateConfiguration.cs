using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class AccountingTemplateConfiguration : IEntityTypeConfiguration<AccountingTemplate>
{
    public void Configure(EntityTypeBuilder<AccountingTemplate> builder)
    {
        builder.ToTable("AccountingTemplates");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Code).IsUnique();
        builder.Property(t => t.EventType).IsRequired().HasMaxLength(100);
        builder.HasIndex(t => t.EventType).IsUnique();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.JournalCode).IsRequired().HasMaxLength(10);
        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.CreatedBy).HasMaxLength(100);
        builder.Property(t => t.UpdatedBy).HasMaxLength(100);
        builder.Property(t => t.DeletedBy).HasMaxLength(100);

        builder.HasMany(t => t.Lines)
               .WithOne()
               .HasForeignKey(l => l.AccountingTemplateId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(t => !t.IsDeleted);

        builder.HasData(
            AccountingTemplate.ForSeed(
                AccountingSeeds.Template_SalesInvoicePosted,
                "SALES_INV_POSTED", "SalesInvoicePosted",
                "Factura de venta contabilizada", "VTA"),
            AccountingTemplate.ForSeed(
                AccountingSeeds.Template_PurchaseInvoicePosted,
                "PURCH_INV_POSTED", "PurchaseInvoicePosted",
                "Factura de compra contabilizada", "CPR"),
            AccountingTemplate.ForSeed(
                AccountingSeeds.Template_CustomerPaymentConfirmed,
                "CUST_PAY_CONFIRMED", "CustomerPaymentConfirmed",
                "Cobro de cliente", "BCO"),
            AccountingTemplate.ForSeed(
                AccountingSeeds.Template_SupplierPaymentConfirmed,
                "SUPP_PAY_CONFIRMED", "SupplierPaymentConfirmed",
                "Pago a proveedor", "BCO")
        );
    }
}
