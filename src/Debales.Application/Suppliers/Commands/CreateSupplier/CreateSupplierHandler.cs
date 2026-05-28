using Debales.Application.Common;
using Debales.Application.Suppliers.DTOs;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers.Commands.CreateSupplier;

public sealed class CreateSupplierHandler
{
    private readonly ISupplierRepository _suppliers;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierHandler(ISupplierRepository suppliers, IUnitOfWork unitOfWork)
    {
        _suppliers = suppliers;
        _unitOfWork = unitOfWork;
    }

    public async Task<SupplierSummaryDto> Handle(CreateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(command.TaxId) &&
            await _suppliers.ExistsByTaxIdAsync(command.TaxId, cancellationToken))
            throw new InvalidOperationException($"Ya existe un proveedor con el CIF/NIF '{command.TaxId}'.");

        var supplier = Supplier.Create(
            command.Name,
            command.TaxId,
            command.Phone,
            command.Email,
            command.ContactName,
            command.CreatedBy);

        await _suppliers.AddAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SupplierSummaryDto(
            supplier.Id, supplier.Name, supplier.TaxId,
            supplier.Phone, supplier.Email, supplier.ContactName,
            supplier.IsActive, supplier.CreatedAt);
    }
}
