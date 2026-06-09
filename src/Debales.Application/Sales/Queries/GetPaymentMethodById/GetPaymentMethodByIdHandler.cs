using Debales.Application.Sales.Commands.CreatePaymentMethod;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Queries.GetPaymentMethodById;

public sealed class GetPaymentMethodByIdHandler(IPaymentMethodRepository repository)
{
    public async Task<PaymentMethodDto?> Handle(GetPaymentMethodByIdQuery query, CancellationToken cancellationToken = default)
    {
        var pm = await repository.GetByIdAsync(query.Id, cancellationToken);
        return pm is null ? null : CreatePaymentMethodHandler.ToDto(pm);
    }
}
