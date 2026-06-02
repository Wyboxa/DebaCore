namespace Debales.Application.Accounting.Queries.GetAccounts;

public sealed record GetAccountsQuery(string? Search, int Page, int PageSize);
