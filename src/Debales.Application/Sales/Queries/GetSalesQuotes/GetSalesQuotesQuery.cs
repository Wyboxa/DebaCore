using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesQuotes;

public sealed record GetSalesQuotesQuery(
    string? Search = null,
    Guid? CustomerId = null,
    SalesQuoteStatus? Status = null,
    int Page = 1,
    int PageSize = 20);
