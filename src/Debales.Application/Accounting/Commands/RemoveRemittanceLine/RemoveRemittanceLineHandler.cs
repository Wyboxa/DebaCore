using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.RemoveRemittanceLine;

public sealed class RemoveRemittanceLineHandler(IRemittanceRepository remittances, IUnitOfWork uow)
{
    public async Task<RemittanceDetailDto> Handle(RemoveRemittanceLineCommand command, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(command.RemittanceId, ct)
            ?? throw new InvalidOperationException("Remesa no encontrada.");

        remittance.RemoveLine(command.DocumentId, command.UpdatedBy);
        await uow.SaveChangesAsync(ct);

        var saved = await remittances.GetByIdAsync(remittance.Id, ct);
        return UpdateRemittanceHandler.ToDto(saved!);
    }
}
