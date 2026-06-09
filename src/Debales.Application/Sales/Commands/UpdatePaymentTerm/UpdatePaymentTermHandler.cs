using Debales.Application.Common;
using Debales.Application.Sales.Commands.CreatePaymentTerm;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Commands.UpdatePaymentTerm;

public sealed class UpdatePaymentTermHandler
{
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IUnitOfWork _uow;

    public UpdatePaymentTermHandler(IPaymentTermRepository paymentTerms, IUnitOfWork uow)
    {
        _paymentTerms = paymentTerms;
        _uow = uow;
    }

    public async Task<PaymentTermDto> Handle(UpdatePaymentTermCommand command, CancellationToken cancellationToken = default)
    {
        var pt = await _paymentTerms.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Condición de pago '{command.Id}' no encontrada.");

        var lines = command.Lines.Select(l => (l.DueDays, l.Percentage));
        pt.Update(command.Name, command.Description, lines, command.UpdatedBy);

        if (command.IsActive) pt.Activate(command.UpdatedBy);
        else pt.Deactivate(command.UpdatedBy);

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _paymentTerms.GetByIdAsync(pt.Id, cancellationToken);
        return CreatePaymentTermHandler.ToDto(saved!);
    }
}
