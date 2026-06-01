using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPayables;

public sealed record GetPayablesQuery(string? Search, Guid? SupplierId, PayableStatus? Status, int Page, int PageSize);
