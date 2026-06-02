using Debales.Application.Suppliers.DTOs;

namespace Debales.Application.AI.ERP;

public sealed record SupplierAIContext(
    SupplierDetailDto Supplier,
    IReadOnlyList<InvoiceSummaryAI> RecentInvoices,
    decimal TotalPurchased,
    IReadOnlyList<PayableSummaryAI> PendingPayables,
    decimal TotalPayable);
