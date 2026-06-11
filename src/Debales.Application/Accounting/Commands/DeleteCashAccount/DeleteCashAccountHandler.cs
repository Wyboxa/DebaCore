using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.DeleteCashAccount;

public sealed class DeleteCashAccountHandler
{
    private readonly ICashAccountRepository _accounts;
    private readonly IUnitOfWork _uow;

    public DeleteCashAccountHandler(ICashAccountRepository accounts, IUnitOfWork uow)
    {
        _accounts = accounts;
        _uow = uow;
    }

    public async Task Handle(DeleteCashAccountCommand command, CancellationToken ct = default)
    {
        var account = await _accounts.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Caja no encontrada.");

        account.Delete(command.DeletedBy);
        await _uow.SaveChangesAsync(ct);
    }
}
