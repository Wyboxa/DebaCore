using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseInvoices;

public sealed record GetPurchaseInvoicesQuery(string? Search, Guid? SupplierId, PurchaseInvoiceStatus? Status, int Page, int PageSize);
