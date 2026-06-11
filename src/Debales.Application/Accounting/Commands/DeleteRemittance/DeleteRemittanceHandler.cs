using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.DeleteRemittance;

public sealed class DeleteRemittanceHandler(IRemittanceRepository remittances, IUnitOfWork uow)
{
    public async Task Handle(DeleteRemittanceCommand command, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Remesa no encontrada.");

        if (remittance.Status == Domain.Accounting.RemittanceStatus.Confirmed)
            throw new InvalidOperationException("No se puede eliminar una remesa ya confirmada.");

        remittance.Delete(command.DeletedBy);
        await uow.SaveChangesAsync(ct);
    }
}
