using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Application.Purchasing;
using Debales.Domain.Accounting;
using Debales.Domain.Sales;
using Debales.Domain.Purchasing;

namespace Debales.Application.Accounting.Commands.ConfirmRemittance;

public sealed class ConfirmRemittanceHandler(
    IRemittanceRepository remittances,
    IReceivableRepository receivables,
    IPayableRepository payables,
    IUnitOfWork uow)
{
    public async Task<RemittanceDetailDto> Handle(ConfirmRemittanceCommand command, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Remesa no encontrada.");

        remittance.Confirm(command.UpdatedBy);

        // Saldar los documentos asociados
        foreach (var line in remittance.Lines)
        {
            if (remittance.Type == RemittanceType.Collection)
            {
                var receivable = await receivables.GetByIdAsync(line.DocumentId, ct);
                if (receivable is not null && receivable.PendingAmount > 0)
                    receivable.ApplyPayment(receivable.PendingAmount, command.UpdatedBy);
            }
            else
            {
                var payable = await payables.GetByIdAsync(line.DocumentId, ct);
                if (payable is not null && payable.PendingAmount > 0)
                    payable.ApplyPayment(payable.PendingAmount, command.UpdatedBy);
            }
        }

        await uow.SaveChangesAsync(ct);

        var saved = await remittances.GetByIdAsync(remittance.Id, ct);
        return UpdateRemittanceHandler.ToDto(saved!);
    }
}
