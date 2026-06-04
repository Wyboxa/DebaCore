using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetJournalBook;

public sealed class GetJournalBookHandler
{
    private readonly IAccountingEntryRepository _entries;

    public GetJournalBookHandler(IAccountingEntryRepository entries) => _entries = entries;

    public async Task<IReadOnlyList<JournalBookEntryDto>> Handle(GetJournalBookQuery query, CancellationToken ct = default)
    {
        var entries = await _entries.GetPostedEntriesWithLinesAsync(query.From, query.To, ct);

        return entries
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Number)
            .Select(e => new JournalBookEntryDto(
                e.Number,
                e.Date,
                e.Journal?.Code ?? string.Empty,
                e.Description,
                e.Status,
                e.Lines
                    .OrderBy(l => l.SortOrder)
                    .Select(l => new JournalBookLineDto(l.AccountCode, l.Description, l.Debit, l.Credit))
                    .ToList(),
                e.Lines.Sum(l => l.Debit),
                e.Lines.Sum(l => l.Credit)))
            .ToList();
    }
}
