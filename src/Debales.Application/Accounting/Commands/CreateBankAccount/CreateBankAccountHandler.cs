using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateBankAccount;

public sealed class CreateBankAccountHandler(IBankAccountRepository repository, IAccountRepository accounts, IUnitOfWork uow)
{
    public async Task<BankAccountDto> Handle(CreateBankAccountCommand command, CancellationToken ct = default)
    {
        var ba = BankAccount.Create(command.Name, command.BankName, command.Iban, command.Bic,
            command.CurrencyCode, command.AccountId, command.CreatedBy);
        await repository.AddAsync(ba, ct);
        await uow.SaveChangesAsync(ct);

        string? accountCode = null, accountName = null;
        if (ba.AccountId.HasValue)
        {
            var acc = await accounts.GetByIdAsync(ba.AccountId.Value, ct);
            accountCode = acc?.Code; accountName = acc?.Name;
        }
        return ToDto(ba, accountCode, accountName);
    }

    internal static BankAccountDto ToDto(BankAccount ba, string? accountCode = null, string? accountName = null) =>
        new(ba.Id, ba.Name, ba.BankName, ba.Iban, ba.Bic, ba.CurrencyCode,
            ba.AccountId, accountCode, accountName, ba.IsActive);
}
