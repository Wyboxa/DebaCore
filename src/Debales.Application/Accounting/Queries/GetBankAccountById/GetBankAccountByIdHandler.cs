using Debales.Application.Accounting.Commands.CreateBankAccount;
using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetBankAccountById;

public sealed class GetBankAccountByIdHandler(IBankAccountRepository repository, IAccountRepository accounts)
{
    public async Task<BankAccountDto?> Handle(GetBankAccountByIdQuery query, CancellationToken ct = default)
    {
        var ba = await repository.GetByIdAsync(query.Id, ct);
        if (ba is null) return null;

        string? accountCode = null, accountName = null;
        if (ba.AccountId.HasValue)
        {
            var acc = await accounts.GetByIdAsync(ba.AccountId.Value, ct);
            accountCode = acc?.Code; accountName = acc?.Name;
        }
        return CreateBankAccountHandler.ToDto(ba, accountCode, accountName);
    }
}
