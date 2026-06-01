using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetReceivables;

public sealed record GetReceivablesQuery(string? Search, Guid? CustomerId, ReceivableStatus? Status, int Page, int PageSize);
