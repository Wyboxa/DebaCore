using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.FailRemittance;

public sealed class FailRemittanceHandler(IRemittanceRepository remittances, IUnitOfWork uow)
{
    public async Task<RemittanceDetailDto> Handle(FailRemittanceCommand command, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Remesa no encontrada.");

        remittance.Fail(command.Reason, command.UpdatedBy);
        await uow.SaveChangesAsync(ct);

        var saved = await remittances.GetByIdAsync(remittance.Id, ct);
        return UpdateRemittanceHandler.ToDto(saved!);
    }
}
