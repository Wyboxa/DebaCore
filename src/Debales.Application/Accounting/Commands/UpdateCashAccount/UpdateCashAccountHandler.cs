using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.UpdateCashAccount;

public sealed class UpdateCashAccountHandler
{
    private readonly ICashAccountRepository _accounts;
    private readonly IUnitOfWork _uow;

    public UpdateCashAccountHandler(ICashAccountRepository accounts, IUnitOfWork uow)
    {
        _accounts = accounts;
        _uow = uow;
    }

    public async Task<CashAccountDetailDto> Handle(UpdateCashAccountCommand command, CancellationToken ct = default)
    {
        var account = await _accounts.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Caja no encontrada.");

        if (await _accounts.ExistsByCodeAsync(command.Code, command.Id, ct))
            throw new InvalidOperationException($"Ya existe una caja con el código '{command.Code}'.");

        account.Update(command.Code, command.Name, command.CurrencyCode,
            command.AccountId, command.IsActive, command.UpdatedBy);

        await _uow.SaveChangesAsync(ct);
        return ToDto(account);
    }

    internal static CashAccountDetailDto ToDto(CashAccount a) => new(
        a.Id, a.Code, a.Name, a.CurrencyCode, a.CurrentBalance,
        a.AccountId, a.Account?.Code, a.Account?.Name, a.IsActive,
        a.CreatedAt, a.CreatedBy, a.UpdatedAt, a.UpdatedBy);
}
