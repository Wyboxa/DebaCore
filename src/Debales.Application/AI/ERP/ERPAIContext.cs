namespace Debales.Application.AI.ERP;

public sealed record ERPAIContext(
    // Ventas
    IReadOnlyList<InvoiceSummaryAI> RecentSalesInvoices,
    decimal TotalSalesThisMonth,
    decimal TotalSalesLastMonth,
    int DraftSalesInvoicesCount,
    // Cobros
    IReadOnlyList<ReceivableSummaryAI> PendingReceivables,
    decimal TotalPendingReceivables,
    int OverdueReceivablesCount,
    // Compras
    IReadOnlyList<InvoiceSummaryAI> RecentPurchaseInvoices,
    decimal TotalPurchasesThisMonth,
    decimal TotalPurchasesLastMonth,
    int DraftPurchaseInvoicesCount,
    // Pagos
    IReadOnlyList<PayableSummaryAI> PendingPayables,
    decimal TotalPendingPayables,
    int OverduePayablesCount,
    // Contabilidad
    int AccountingEntriesThisMonth,
    int UnpostedInvoicesWithAccount);

public sealed record InvoiceSummaryAI(
    string Number, string ThirdParty, DateOnly Date, DateOnly DueDate,
    decimal Total, string Status);

public sealed record ReceivableSummaryAI(
    string Number, string Customer, DateOnly DueDate,
    decimal Amount, string Status, bool IsOverdue);

public sealed record PayableSummaryAI(
    string Number, string Supplier, DateOnly DueDate,
    decimal Amount, string Status, bool IsOverdue);
