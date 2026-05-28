using Debales.Application.Suppliers.Commands.UpdateSupplier;
using Debales.Application.Suppliers.DTOs;

namespace Debales.Application.Suppliers.Queries.GetSupplierById;

public sealed class GetSupplierByIdHandler
{
    private readonly ISupplierRepository _suppliers;

    public GetSupplierByIdHandler(ISupplierRepository suppliers) => _suppliers = suppliers;

    public async Task<SupplierDetailDto?> Handle(GetSupplierByIdQuery query, CancellationToken cancellationToken = default)
    {
        var supplier = await _suppliers.GetByIdAsync(query.Id, cancellationToken);
        if (supplier is null) return null;

        return UpdateSupplierHandler.ToDto(supplier);
    }
}
