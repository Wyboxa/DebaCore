using Debales.Application.Accounting.DTOs;
using Debales.Application.Accounting.Queries.GetAccountingEntryById;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Queries.GetAccountingEntries;

public sealed class GetAccountingEntriesHandler
{
    private readonly IAccountingEntryRepository _entries;

    public GetAccountingEntriesHandler(IAccountingEntryRepository entries) => _entries = entries;

    public async Task<PagedResult<AccountingEntrySummaryDto>> Handle(GetAccountingEntriesQuery query, CancellationToken ct = default)
    {
        var result = await _entries.SearchAsync(query.Search, query.JournalId, query.FiscalPeriodId, query.Page, query.PageSize, ct);
        return new PagedResult<AccountingEntrySummaryDto>(
            result.Items.Select(GetAccountingEntryByIdHandler.ToSummaryDto).ToList(),
            result.TotalCount, result.Page, result.PageSize);
    }
}
