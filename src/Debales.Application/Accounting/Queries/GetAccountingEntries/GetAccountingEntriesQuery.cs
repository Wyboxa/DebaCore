namespace Debales.Application.Accounting.Queries.GetAccountingEntries;

public sealed record GetAccountingEntriesQuery(
    string? Search, Guid? JournalId, Guid? FiscalPeriodId, int Page, int PageSize);
