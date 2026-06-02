using Debales.Application.Accounting;
using Debales.Application.AI.Chat;
using Debales.Application.Purchasing;
using Debales.Application.Sales;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;

namespace Debales.Application.AI.ERP;

public sealed class ChatWithERPHandler
{
    private readonly IAIService _ai;
    private readonly ISalesInvoiceRepository _salesInvoices;
    private readonly IReceivableRepository _receivables;
    private readonly IPurchaseInvoiceRepository _purchaseInvoices;
    private readonly IPayableRepository _payables;
    private readonly IAccountingEntryRepository _entries;

    public ChatWithERPHandler(
        IAIService ai,
        ISalesInvoiceRepository salesInvoices,
        IReceivableRepository receivables,
        IPurchaseInvoiceRepository purchaseInvoices,
        IPayableRepository payables,
        IAccountingEntryRepository entries)
    {
        _ai = ai;
        _salesInvoices = salesInvoices;
        _receivables = receivables;
        _purchaseInvoices = purchaseInvoices;
        _payables = payables;
        _entries = entries;
    }

    public async Task<ChatResponseDto> Handle(ChatWithERPCommand command, CancellationToken ct = default)
    {
        var context = await BuildContextAsync(ct);
        var reply = await _ai.ChatWithERPAsync(context, command.History, command.NewMessage, ct);
        return new ChatResponseDto(reply);
    }

    internal async Task<ERPAIContext> BuildContextAsync(CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var thisMonthStart = new DateOnly(today.Year, today.Month, 1);
        var lastMonthStart = thisMonthStart.AddMonths(-1);

        var salesResult        = await _salesInvoices.SearchAsync(null, null, SalesInvoiceStatus.Posted, 1, 50, ct);
        var purchaseResult     = await _purchaseInvoices.SearchAsync(null, null, PurchaseInvoiceStatus.Posted, 1, 50, ct);
        var receivablesResult  = await _receivables.SearchAsync(null, null, ReceivableStatus.Pending, 1, 100, ct);
        var payablesResult     = await _payables.SearchAsync(null, null, PayableStatus.Pending, 1, 100, ct);
        var entriesResult      = await _entries.SearchAsync(null, null, null, 1, 1, ct);
        var draftSales         = await _salesInvoices.SearchAsync(null, null, SalesInvoiceStatus.Draft, 1, 1, ct);
        var draftPurchases     = await _purchaseInvoices.SearchAsync(null, null, PurchaseInvoiceStatus.Draft, 1, 1, ct);

        var salesInvoices = salesResult.Items
            .Select(i => new InvoiceSummaryAI(i.Number, i.Customer?.Name ?? "", i.Date, i.DueDate, i.Total, "Contabilizada"))
            .ToList();

        var purchaseInvoices = purchaseResult.Items
            .Select(i => new InvoiceSummaryAI(i.Number, i.Supplier?.Name ?? "", i.Date, i.DueDate, i.Total, "Contabilizada"))
            .ToList();

        var receivables = receivablesResult.Items
            .Select(r => new ReceivableSummaryAI(r.Number, r.Customer?.Name ?? "", r.DueDate, r.OriginalAmount, "Pendiente", r.DueDate < today))
            .ToList();

        var payables = payablesResult.Items
            .Select(p => new PayableSummaryAI(p.Number, p.Supplier?.Name ?? "", p.DueDate, p.OriginalAmount, "Pendiente", p.DueDate < today))
            .ToList();

        return new ERPAIContext(
            RecentSalesInvoices: salesInvoices,
            TotalSalesThisMonth: salesInvoices.Where(i => i.Date >= thisMonthStart).Sum(i => i.Total),
            TotalSalesLastMonth: salesInvoices.Where(i => i.Date >= lastMonthStart && i.Date < thisMonthStart).Sum(i => i.Total),
            DraftSalesInvoicesCount: draftSales.TotalCount,
            PendingReceivables: receivables,
            TotalPendingReceivables: receivables.Sum(r => r.Amount),
            OverdueReceivablesCount: receivables.Count(r => r.IsOverdue),
            RecentPurchaseInvoices: purchaseInvoices,
            TotalPurchasesThisMonth: purchaseInvoices.Where(i => i.Date >= thisMonthStart).Sum(i => i.Total),
            TotalPurchasesLastMonth: purchaseInvoices.Where(i => i.Date >= lastMonthStart && i.Date < thisMonthStart).Sum(i => i.Total),
            DraftPurchaseInvoicesCount: draftPurchases.TotalCount,
            PendingPayables: payables,
            TotalPendingPayables: payables.Sum(p => p.Amount),
            OverduePayablesCount: payables.Count(p => p.IsOverdue),
            AccountingEntriesThisMonth: entriesResult.TotalCount,
            UnpostedInvoicesWithAccount: draftSales.TotalCount + draftPurchases.TotalCount);
    }
}
