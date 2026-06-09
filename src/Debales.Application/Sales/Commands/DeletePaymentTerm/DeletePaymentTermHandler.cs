using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.DeletePaymentTerm;

public sealed class DeletePaymentTermHandler
{
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IUnitOfWork _uow;

    public DeletePaymentTermHandler(IPaymentTermRepository paymentTerms, IUnitOfWork uow)
    {
        _paymentTerms = paymentTerms;
        _uow = uow;
    }

    public async Task Handle(DeletePaymentTermCommand command, CancellationToken cancellationToken = default)
    {
        var pt = await _paymentTerms.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Condición de pago '{command.Id}' no encontrada.");

        pt.Delete(command.DeletedBy);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
