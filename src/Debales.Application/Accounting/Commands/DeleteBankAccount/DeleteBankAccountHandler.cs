using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.DeleteBankAccount;

public sealed class DeleteBankAccountHandler(IBankAccountRepository repository, IUnitOfWork uow)
{
    public async Task Handle(DeleteBankAccountCommand command, CancellationToken ct = default)
    {
        var ba = await repository.GetByIdAsync(command.Id, ct)
            ?? throw new KeyNotFoundException($"Cuenta bancaria {command.Id} no encontrada.");
        ba.Delete(command.DeletedBy);
        await uow.SaveChangesAsync(ct);
    }
}
