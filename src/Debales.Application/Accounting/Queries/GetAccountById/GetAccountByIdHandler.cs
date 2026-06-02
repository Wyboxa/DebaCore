using Debales.Application.Accounting.Commands.CreateAccount;
using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetAccountById;

public sealed class GetAccountByIdHandler
{
    private readonly IAccountRepository _accounts;

    public GetAccountByIdHandler(IAccountRepository accounts) => _accounts = accounts;

    public async Task<AccountDetailDto?> Handle(GetAccountByIdQuery query, CancellationToken ct = default)
    {
        var account = await _accounts.GetByIdAsync(query.Id, ct);
        return account is null ? null : CreateAccountHandler.ToDetailDto(account);
    }
}
