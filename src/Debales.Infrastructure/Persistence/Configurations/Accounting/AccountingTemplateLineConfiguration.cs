using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Debales.Infrastructure.Persistence.Configurations.Accounting;

internal sealed class AccountingTemplateLineConfiguration : IEntityTypeConfiguration<AccountingTemplateLine>
{
    public void Configure(EntityTypeBuilder<AccountingTemplateLine> builder)
    {
        builder.ToTable("AccountingTemplateLines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.AccountingTemplateId).IsRequired();
        builder.Property(l => l.SortOrder).IsRequired();
        builder.Property(l => l.Side).IsRequired();
        builder.Property(l => l.AccountCode).IsRequired().HasMaxLength(30);
        builder.Property(l => l.AmountType).IsRequired();
        builder.Property(l => l.Description).IsRequired().HasMaxLength(200);
        builder.Property(l => l.CreatedAt).IsRequired();

        builder.HasData(
            // Factura venta: D: {CUSTOMER} Total  H: 700 Base  H: 477 Tax
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_Sales_D_Customer,
                AccountingSeeds.Template_SalesInvoicePosted,
                1, TemplateSide.Debit, "{CUSTOMER}", TemplateAmountType.Total,
                "Cliente — total factura"),
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_Sales_H_700,
                AccountingSeeds.Template_SalesInvoicePosted,
                2, TemplateSide.Credit, "700", TemplateAmountType.Base,
                "Ventas de mercaderías"),
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_Sales_H_477,
                AccountingSeeds.Template_SalesInvoicePosted,
                3, TemplateSide.Credit, "477", TemplateAmountType.Tax,
                "IVA repercutido"),

            // Factura compra: D: 600 Base  D: 472 Tax  H: {SUPPLIER} Total
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_Purchase_D_600,
                AccountingSeeds.Template_PurchaseInvoicePosted,
                1, TemplateSide.Debit, "600", TemplateAmountType.Base,
                "Compras de mercaderías"),
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_Purchase_D_472,
                AccountingSeeds.Template_PurchaseInvoicePosted,
                2, TemplateSide.Debit, "472", TemplateAmountType.Tax,
                "IVA soportado"),
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_Purchase_H_Supplier,
                AccountingSeeds.Template_PurchaseInvoicePosted,
                3, TemplateSide.Credit, "{SUPPLIER}", TemplateAmountType.Total,
                "Proveedor — total factura"),

            // Cobro cliente: D: 572 Bancos Total  H: {CUSTOMER} Total
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_CustPay_D_572,
                AccountingSeeds.Template_CustomerPaymentConfirmed,
                1, TemplateSide.Debit, "572", TemplateAmountType.Total,
                "Bancos — cobro recibido"),
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_CustPay_H_Customer,
                AccountingSeeds.Template_CustomerPaymentConfirmed,
                2, TemplateSide.Credit, "{CUSTOMER}", TemplateAmountType.Total,
                "Cliente — cancelación de deuda"),

            // Pago proveedor: D: {SUPPLIER} Total  H: 572 Bancos Total
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_SuppPay_D_Supplier,
                AccountingSeeds.Template_SupplierPaymentConfirmed,
                1, TemplateSide.Debit, "{SUPPLIER}", TemplateAmountType.Total,
                "Proveedor — cancelación de deuda"),
            AccountingTemplateLine.ForSeed(
                AccountingSeeds.TemplateLine_SuppPay_H_572,
                AccountingSeeds.Template_SupplierPaymentConfirmed,
                2, TemplateSide.Credit, "572", TemplateAmountType.Total,
                "Bancos — pago realizado")
        );
    }
}
