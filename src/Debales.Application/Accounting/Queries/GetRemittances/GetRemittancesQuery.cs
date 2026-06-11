using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Queries.GetRemittances;

public sealed record GetRemittancesQuery(
    RemittanceType? Type, RemittanceStatus? Status, int Page, int PageSize);
