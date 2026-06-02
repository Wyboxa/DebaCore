using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Services;

public interface IAccountingEntryService
{
    Task<AccountingEntry?> GenerateFromSalesInvoiceAsync(
        Guid invoiceId, string invoiceNumber, DateOnly invoiceDate,
        Guid customerId, string? customerAccountCode,
        decimal baseAmount, decimal taxAmount, decimal total,
        CancellationToken ct = default);

    Task<AccountingEntry?> GenerateFromPurchaseInvoiceAsync(
        Guid invoiceId, string invoiceNumber, DateOnly invoiceDate,
        Guid supplierId, string? supplierAccountCode,
        decimal baseAmount, decimal taxAmount, decimal total,
        CancellationToken ct = default);
}
