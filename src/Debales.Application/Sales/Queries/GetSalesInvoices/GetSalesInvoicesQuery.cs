using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesInvoices;

public sealed record GetSalesInvoicesQuery(
    string? Search, Guid? CustomerId, SalesInvoiceStatus? Status, int Page, int PageSize);
