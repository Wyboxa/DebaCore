using Debales.Application.Accounting.DTOs;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Queries.GetAccountingEntryById;

public sealed class GetAccountingEntryByIdHandler
{
    private readonly IAccountingEntryRepository _entries;

    public GetAccountingEntryByIdHandler(IAccountingEntryRepository entries) => _entries = entries;

    public async Task<AccountingEntryDetailDto?> Handle(GetAccountingEntryByIdQuery query, CancellationToken ct = default)
    {
        var entry = await _entries.GetByIdWithLinesAsync(query.Id, ct);
        return entry is null ? null : ToDto(entry);
    }

    internal static AccountingEntryDetailDto ToDto(AccountingEntry e) => new(
        e.Id, e.Number, e.Date, e.Description,
        e.JournalId, e.Journal?.Code ?? string.Empty, e.Journal?.Name ?? string.Empty,
        e.FiscalPeriodId, e.FiscalPeriod?.Name ?? string.Empty,
        e.Status, StatusLabel(e.Status),
        e.TotalDebit, e.TotalCredit, e.IsBalanced,
        e.SourceType, e.SourceId,
        e.Lines.Select(ToLineDto).ToList(),
        e.CreatedAt, e.CreatedBy, e.UpdatedAt, e.UpdatedBy);

    internal static AccountingEntrySummaryDto ToSummaryDto(AccountingEntry e) => new(
        e.Id, e.Number, e.Date, e.Description,
        e.JournalId, e.Journal?.Code ?? string.Empty, e.Journal?.Name ?? string.Empty,
        e.Status, StatusLabel(e.Status),
        e.TotalDebit, e.TotalCredit,
        e.SourceType, e.SourceId);

    private static AccountingEntryLineDto ToLineDto(AccountingEntryLine l) => new(
        l.Id, l.SortOrder, l.AccountId, l.AccountCode,
        l.Description, l.Debit, l.Credit,
        l.ThirdPartyId, l.ThirdPartyType);

    internal static string StatusLabel(EntryStatus s) => s switch
    {
        EntryStatus.Draft => "Borrador",
        EntryStatus.Posted => "Contabilizado",
        EntryStatus.Cancelled => "Anulado",
        _ => s.ToString()
    };
}
