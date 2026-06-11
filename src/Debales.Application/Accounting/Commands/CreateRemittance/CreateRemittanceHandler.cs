using Debales.Application.Accounting.Commands.UpdateRemittance;
using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateRemittance;

public sealed class CreateRemittanceHandler(IRemittanceRepository remittances, IBankAccountRepository banks, IUnitOfWork uow)
{
    public async Task<RemittanceDetailDto> Handle(CreateRemittanceCommand command, CancellationToken ct = default)
    {
        var bank = await banks.GetByIdAsync(command.BankAccountId, ct)
            ?? throw new InvalidOperationException("Cuenta bancaria no encontrada.");

        var number = await remittances.GetNextNumberAsync(command.Type, ct);
        var remittance = Remittance.Create(number, command.Date, command.BankAccountId,
            command.Type, command.Notes, command.CreatedBy);

        await remittances.AddAsync(remittance, ct);
        await uow.SaveChangesAsync(ct);

        var saved = await remittances.GetByIdAsync(remittance.Id, ct);
        return UpdateRemittanceHandler.ToDto(saved!);
    }
}
