using Debales.Application.Sales.Commands.CreatePaymentTerm;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Queries.GetPaymentTermById;

public sealed class GetPaymentTermByIdHandler
{
    private readonly IPaymentTermRepository _paymentTerms;

    public GetPaymentTermByIdHandler(IPaymentTermRepository paymentTerms) => _paymentTerms = paymentTerms;

    public async Task<PaymentTermDto?> Handle(GetPaymentTermByIdQuery query, CancellationToken cancellationToken = default)
    {
        var pt = await _paymentTerms.GetByIdAsync(query.Id, cancellationToken);
        return pt is null ? null : CreatePaymentTermHandler.ToDto(pt);
    }
}
