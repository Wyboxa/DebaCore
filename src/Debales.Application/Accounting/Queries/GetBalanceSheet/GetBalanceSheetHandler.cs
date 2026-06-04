using Debales.Application.Accounting.DTOs;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Queries.GetBalanceSheet;

public sealed class GetBalanceSheetHandler
{
    private readonly IAccountingEntryRepository _entries;

    public GetBalanceSheetHandler(IAccountingEntryRepository entries) => _entries = entries;

    public async Task<BalanceSheetDto> Handle(GetBalanceSheetQuery query, CancellationToken ct = default)
    {
        var data = await _entries.GetTrialBalanceDataAsync(null, query.To, ct);

        static decimal NaturalBalance(AccountType t, decimal debit, decimal credit) => t switch
        {
            AccountType.Asset or AccountType.Expense => debit - credit,
            _ => credit - debit
        };

        static string TypeLabel(AccountType t) => t switch
        {
            AccountType.Asset => "Activo",
            AccountType.Liability => "Pasivo",
            AccountType.Equity => "Patrimonio neto",
            AccountType.Revenue => "Ingresos",
            AccountType.Expense => "Gastos",
            _ => t.ToString()
        };

        var groups = data
            .Select(r => (
                Type: Enum.Parse<AccountType>(r.AccountType),
                Code: r.AccountCode,
                Name: r.AccountName,
                Balance: NaturalBalance(Enum.Parse<AccountType>(r.AccountType), r.TotalDebit, r.TotalCredit)))
            .Where(r => r.Balance != 0)
            .GroupBy(r => r.Type)
            .OrderBy(g => (int)g.Key)
            .Select(g => new BalanceSheetGroupDto(
                g.Key,
                TypeLabel(g.Key),
                g.OrderBy(r => r.Code)
                    .Select(r => new BalanceSheetLineDto(r.Code, r.Name, r.Balance))
                    .ToList(),
                g.Sum(r => r.Balance)))
            .ToList();

        var assets = groups.Where(g => g.Type == AccountType.Asset).Sum(g => g.Total);
        var liabilitiesAndEquity = groups
            .Where(g => g.Type is AccountType.Liability or AccountType.Equity)
            .Sum(g => g.Total);

        return new BalanceSheetDto(
            query.To,
            groups,
            assets,
            liabilitiesAndEquity,
            Math.Abs(assets - liabilitiesAndEquity) < 0.01m);
    }
}
