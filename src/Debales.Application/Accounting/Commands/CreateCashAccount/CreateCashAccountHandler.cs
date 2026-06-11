using Debales.Application.Accounting.Commands.UpdateCashAccount;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateCashAccount;

public sealed class CreateCashAccountHandler
{
    private readonly ICashAccountRepository _accounts;
    private readonly IUnitOfWork _uow;

    public CreateCashAccountHandler(ICashAccountRepository accounts, IUnitOfWork uow)
    {
        _accounts = accounts;
        _uow = uow;
    }

    public async Task<CashAccountDetailDto> Handle(CreateCashAccountCommand command, CancellationToken ct = default)
    {
        if (await _accounts.ExistsByCodeAsync(command.Code, null, ct))
            throw new InvalidOperationException($"Ya existe una caja con el código '{command.Code}'.");

        var account = CashAccount.Create(command.Code, command.Name, command.CurrencyCode,
            command.AccountId, command.CreatedBy);

        await _accounts.AddAsync(account, ct);
        await _uow.SaveChangesAsync(ct);

        var saved = await _accounts.GetByIdAsync(account.Id, ct);
        return UpdateCashAccountHandler.ToDto(saved!);
    }
}
