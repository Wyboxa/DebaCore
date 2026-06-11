using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.AddRemittanceLine;

public sealed class AddRemittanceLineHandler(IRemittanceRepository remittances, IUnitOfWork uow)
{
    public async Task<RemittanceDetailDto> Handle(AddRemittanceLineCommand command, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(command.RemittanceId, ct)
            ?? throw new InvalidOperationException("Remesa no encontrada.");

        remittance.AddLine(command.DocumentId, command.Amount, command.UpdatedBy);
        await uow.SaveChangesAsync(ct);

        var saved = await remittances.GetByIdAsync(remittance.Id, ct);
        return UpdateRemittanceHandler.ToDto(saved!);
    }
}
