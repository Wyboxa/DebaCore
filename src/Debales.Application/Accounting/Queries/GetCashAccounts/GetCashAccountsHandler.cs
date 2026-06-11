using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Queries.GetCashAccounts;

public sealed class GetCashAccountsHandler(ICashAccountRepository repository)
{
    public async Task<PagedResult<CashAccountSummaryDto>> Handle(GetCashAccountsQuery query, CancellationToken ct = default)
    {
        var paged = await repository.SearchAsync(query.Search, query.IsActive, query.Page, query.PageSize, ct);
        var items = paged.Items.Select(a =>
            new CashAccountSummaryDto(a.Id, a.Code, a.Name, a.CurrencyCode, a.CurrentBalance, a.IsActive)).ToList();
        return new PagedResult<CashAccountSummaryDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
