using Debales.Application.Accounting.DTOs;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Queries.GetTrialBalance;

public sealed class GetTrialBalanceHandler
{
    private readonly IAccountingEntryRepository _entries;

    public GetTrialBalanceHandler(IAccountingEntryRepository entries) => _entries = entries;

    public async Task<TrialBalanceDto> Handle(GetTrialBalanceQuery query, CancellationToken ct = default)
    {
        var data = await _entries.GetTrialBalanceDataAsync(query.From, query.To, ct);

        var lines = data
            .OrderBy(r => r.AccountCode)
            .Select(r => new TrialBalanceLineDto(
                r.AccountCode,
                r.AccountName,
                Enum.Parse<AccountType>(r.AccountType),
                r.TotalDebit,
                r.TotalCredit,
                r.TotalDebit - r.TotalCredit))
            .ToList();

        return new TrialBalanceDto(
            string.Empty,
            query.From,
            query.To,
            lines,
            lines.Sum(l => l.TotalDebit),
            lines.Sum(l => l.TotalCredit));
    }
}
