using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.DeletePaymentMethod;

public sealed class DeletePaymentMethodHandler
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeletePaymentMethodHandler(IPaymentMethodRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeletePaymentMethodCommand command, CancellationToken cancellationToken = default)
    {
        var pm = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Forma de pago {command.Id} no encontrada.");
        pm.Delete(command.DeletedBy);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
