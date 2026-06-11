using Debales.Application.Accounting.Commands.CreateBankAccount;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.UpdateBankAccount;

public sealed class UpdateBankAccountHandler(IBankAccountRepository repository, IAccountRepository accounts, IUnitOfWork uow)
{
    public async Task<BankAccountDto> Handle(UpdateBankAccountCommand command, CancellationToken ct = default)
    {
        var ba = await repository.GetByIdAsync(command.Id, ct)
            ?? throw new KeyNotFoundException($"Cuenta bancaria {command.Id} no encontrada.");

        ba.Update(command.Name, command.BankName, command.Iban, command.Bic,
            command.CurrencyCode, command.AccountId, command.IsActive, command.UpdatedBy);
        await uow.SaveChangesAsync(ct);

        string? accountCode = null, accountName = null;
        if (ba.AccountId.HasValue)
        {
            var acc = await accounts.GetByIdAsync(ba.AccountId.Value, ct);
            accountCode = acc?.Code; accountName = acc?.Name;
        }
        return CreateBankAccountHandler.ToDto(ba, accountCode, accountName);
    }
}
