using Debales.Application.Accounting.Commands.UpdateCashAccount;
using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetCashAccountById;

public sealed class GetCashAccountByIdHandler(ICashAccountRepository repository)
{
    public async Task<CashAccountDetailDto?> Handle(GetCashAccountByIdQuery query, CancellationToken ct = default)
    {
        var account = await repository.GetByIdAsync(query.Id, ct);
        return account is null ? null : UpdateCashAccountHandler.ToDto(account);
    }
}
