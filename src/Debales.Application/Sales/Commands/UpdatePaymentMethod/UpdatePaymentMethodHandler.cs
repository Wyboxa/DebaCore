using Debales.Application.Common;
using Debales.Application.Sales.Commands.CreatePaymentMethod;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Commands.UpdatePaymentMethod;

public sealed class UpdatePaymentMethodHandler
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IUnitOfWork _uow;

    public UpdatePaymentMethodHandler(IPaymentMethodRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<PaymentMethodDto> Handle(UpdatePaymentMethodCommand command, CancellationToken cancellationToken = default)
    {
        var pm = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Forma de pago {command.Id} no encontrada.");
        pm.Update(command.Name, command.Code, command.Description, command.IsActive, command.UpdatedBy);
        await _uow.SaveChangesAsync(cancellationToken);
        return CreatePaymentMethodHandler.ToDto(pm);
    }
}
