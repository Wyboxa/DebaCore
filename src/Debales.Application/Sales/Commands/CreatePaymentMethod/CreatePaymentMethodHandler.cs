using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreatePaymentMethod;

public sealed class CreatePaymentMethodHandler
{
    private readonly IPaymentMethodRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreatePaymentMethodHandler(IPaymentMethodRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<PaymentMethodDto> Handle(CreatePaymentMethodCommand command, CancellationToken cancellationToken = default)
    {
        var pm = PaymentMethod.Create(command.Name, command.Code, command.Description, command.CreatedBy);
        await _repository.AddAsync(pm, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        var saved = await _repository.GetByIdAsync(pm.Id, cancellationToken) ?? pm;
        return ToDto(saved);
    }

    internal static PaymentMethodDto ToDto(PaymentMethod pm) =>
        new(pm.Id, pm.Name, pm.Code, pm.Description, pm.IsActive);
}
