using Debales.Application.Sales;
using Debales.Application.Suppliers.Commands.UpdateSupplier;
using Debales.Application.Suppliers.DTOs;

namespace Debales.Application.Suppliers.Queries.GetSupplierById;

public sealed class GetSupplierByIdHandler
{
    private readonly ISupplierRepository _suppliers;
    private readonly IPaymentTermRepository _paymentTerms;

    public GetSupplierByIdHandler(ISupplierRepository suppliers, IPaymentTermRepository paymentTerms)
    {
        _suppliers = suppliers;
        _paymentTerms = paymentTerms;
    }

    public async Task<SupplierDetailDto?> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken = default)
    {
        var supplier = await _suppliers.GetByIdAsync(query.Id, cancellationToken);
        if (supplier is null) return null;

        string? paymentTermName = null;
        if (supplier.PaymentTermId.HasValue)
        {
            var pt = await _paymentTerms.GetByIdAsync(supplier.PaymentTermId.Value, cancellationToken);
            paymentTermName = pt?.Name;
        }

        return UpdateSupplierHandler.ToDto(supplier, paymentTermName);
    }
}
