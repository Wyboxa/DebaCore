using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.UpdateRemittance;

public sealed class UpdateRemittanceHandler(IRemittanceRepository remittances, IUnitOfWork uow)
{
    public async Task<RemittanceDetailDto> Handle(UpdateRemittanceCommand command, CancellationToken ct = default)
    {
        var remittance = await remittances.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Remesa no encontrada.");

        remittance.UpdateNotes(command.Notes, command.UpdatedBy);

        await uow.SaveChangesAsync(ct);
        return ToDto(remittance);
    }

    internal static RemittanceDetailDto ToDto(Remittance r) => new(
        r.Id, r.Number, r.Date,
        r.BankAccountId, r.BankAccount?.Name ?? "",
        r.Type, r.Type == RemittanceType.Collection ? "Cobro" : "Pago",
        r.Status, StatusLabel(r.Status),
        r.Notes, r.FailReason,
        r.Lines.Select(l => new RemittanceLineDto(l.Id, l.DocumentId, l.Amount)).ToList(),
        r.Total,
        r.CreatedAt, r.CreatedBy, r.UpdatedAt, r.UpdatedBy);

    private static string StatusLabel(RemittanceStatus s) => s switch
    {
        RemittanceStatus.Draft     => "Borrador",
        RemittanceStatus.Sent      => "Enviada",
        RemittanceStatus.Confirmed => "Confirmada",
        RemittanceStatus.Failed    => "Fallida",
        _                          => s.ToString()
    };
}
