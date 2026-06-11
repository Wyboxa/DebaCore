using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Queries.GetBankAccounts;

public sealed class GetBankAccountsHandler(IBankAccountRepository repository)
{
    public async Task<PagedResult<BankAccountSummaryDto>> Handle(GetBankAccountsQuery query, CancellationToken ct = default)
    {
        var paged = await repository.SearchAsync(query.Search, query.IsActive, query.Page, query.PageSize, ct);
        var items = paged.Items.Select(ba =>
            new BankAccountSummaryDto(ba.Id, ba.Name, ba.BankName, ba.Iban, ba.CurrencyCode, ba.IsActive)).ToList();
        return new PagedResult<BankAccountSummaryDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
