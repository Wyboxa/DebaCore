using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Services;

public sealed class AccountingEntryService : IAccountingEntryService
{
    private readonly IAccountRepository _accounts;
    private readonly IFiscalYearRepository _fiscalYears;
    private readonly IAccountingJournalRepository _journals;
    private readonly IAccountingEntryRepository _entries;
    private readonly IAccountingTemplateRepository _templates;

    public AccountingEntryService(
        IAccountRepository accounts,
        IFiscalYearRepository fiscalYears,
        IAccountingJournalRepository journals,
        IAccountingEntryRepository entries,
        IAccountingTemplateRepository templates)
    {
        _accounts = accounts;
        _fiscalYears = fiscalYears;
        _journals = journals;
        _entries = entries;
        _templates = templates;
    }

    public Task<AccountingEntry?> GenerateFromSalesInvoiceAsync(
        Guid invoiceId, string invoiceNumber, DateOnly invoiceDate,
        Guid customerId, string? customerAccountCode,
        decimal baseAmount, decimal taxAmount, decimal total,
        CancellationToken ct = default) =>
        GenerateAsync(
            "SalesInvoicePosted", invoiceId, invoiceNumber, invoiceDate,
            $"Factura venta {invoiceNumber}",
            "{CUSTOMER}", customerId, customerAccountCode,
            baseAmount, taxAmount, total, "Customer", ct);

    public Task<AccountingEntry?> GenerateFromPurchaseInvoiceAsync(
        Guid invoiceId, string invoiceNumber, DateOnly invoiceDate,
        Guid supplierId, string? supplierAccountCode,
        decimal baseAmount, decimal taxAmount, decimal total,
        CancellationToken ct = default) =>
        GenerateAsync(
            "PurchaseInvoicePosted", invoiceId, invoiceNumber, invoiceDate,
            $"Factura compra {invoiceNumber}",
            "{SUPPLIER}", supplierId, supplierAccountCode,
            baseAmount, taxAmount, total, "Supplier", ct);

    private async Task<AccountingEntry?> GenerateAsync(
        string eventType, Guid sourceId, string sourceNumber, DateOnly date,
        string description, string thirdPartyToken, Guid thirdPartyId,
        string? thirdPartyAccountCode,
        decimal baseAmount, decimal taxAmount, decimal total,
        string thirdPartyType, CancellationToken ct)
    {
        var template = await _templates.GetByEventTypeAsync(eventType, ct);
        if (template is null || !template.IsActive) return null;

        var period = await _fiscalYears.GetOpenPeriodForDateAsync(date, ct);
        if (period is null) return null;

        var journal = await _journals.GetByCodeAsync(template.JournalCode, ct);
        if (journal is null || !journal.IsActive) return null;

        var number = await _entries.GetNextNumberAsync(journal.Id, journal.Code, ct);
        var entry = AccountingEntry.Create(number, date, description, journal.Id, period.Id, eventType, sourceId, "system");

        foreach (var tl in template.Lines.OrderBy(l => l.SortOrder))
        {
            var accountCode = tl.AccountCode;
            Guid? linkedThirdParty = null;
            string? linkedThirdPartyType = null;

            if (accountCode == thirdPartyToken)
            {
                if (thirdPartyAccountCode is null) return null;
                accountCode = thirdPartyAccountCode;
                linkedThirdParty = thirdPartyId;
                linkedThirdPartyType = thirdPartyType;
            }

            var account = await _accounts.GetByCodeAsync(accountCode, ct);
            if (account is null) return null;

            var amount = tl.AmountType switch
            {
                TemplateAmountType.Total => total,
                TemplateAmountType.Base => baseAmount,
                TemplateAmountType.Tax => taxAmount,
                _ => 0m
            };

            var debit = tl.Side == TemplateSide.Debit ? amount : 0m;
            var credit = tl.Side == TemplateSide.Credit ? amount : 0m;

            entry.AddLine(account.Id, account.Code, tl.Description, debit, credit, linkedThirdParty, linkedThirdPartyType);
        }

        if (!entry.IsBalanced) return null;

        await _entries.AddAsync(entry, ct);
        return entry;
    }
}
