namespace Debales.Application.Accounting.Queries.GetBankAccounts;

public sealed record GetBankAccountsQuery(string? Search = null, bool? IsActive = null, int Page = 1, int PageSize = 50);
