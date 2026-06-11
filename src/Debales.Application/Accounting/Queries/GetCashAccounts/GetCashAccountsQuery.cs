namespace Debales.Application.Accounting.Queries.GetCashAccounts;

public sealed record GetCashAccountsQuery(string? Search, bool? IsActive, int Page, int PageSize);
