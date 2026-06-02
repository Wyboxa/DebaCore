using Debales.Application.Accounting.Commands.CreateAccount;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Queries.GetAccounts;

public sealed class GetAccountsHandler
{
    private readonly IAccountRepository _accounts;

    public GetAccountsHandler(IAccountRepository accounts) => _accounts = accounts;

    public async Task<PagedResult<AccountSummaryDto>> Handle(GetAccountsQuery query, CancellationToken ct = default)
    {
        var result = await _accounts.SearchAsync(query.Search, query.Page, query.PageSize, ct);
        return new PagedResult<AccountSummaryDto>(
            result.Items.Select(CreateAccountHandler.ToDto).ToList(),
            result.TotalCount, result.Page, result.PageSize);
    }
}
