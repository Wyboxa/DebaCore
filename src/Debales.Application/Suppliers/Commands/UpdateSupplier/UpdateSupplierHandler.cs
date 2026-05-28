using Debales.Application.Common;
using Debales.Application.Suppliers.DTOs;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers.Commands.UpdateSupplier;

public sealed class UpdateSupplierHandler
{
    private readonly ISupplierRepository _suppliers;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSupplierHandler(ISupplierRepository suppliers, IUnitOfWork unitOfWork)
    {
        _suppliers = suppliers;
        _unitOfWork = unitOfWork;
    }

    public async Task<SupplierDetailDto> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken = default)
    {
        var supplier = await _suppliers.GetByIdAsync(command.SupplierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Proveedor '{command.SupplierId}' no encontrado.");

        supplier.Update(
            command.Name, command.TaxId, command.Phone, command.Email,
            command.Website, command.ContactName, command.Notes, command.UpdatedBy);

        if (!string.IsNullOrWhiteSpace(command.AddressCity))
        {
            var address = SupplierAddress.Create(
                command.AddressStreet ?? string.Empty,
                command.AddressCity,
                command.AddressPostalCode ?? string.Empty,
                command.AddressCountry ?? string.Empty);
            supplier.SetAddress(address, command.UpdatedBy);
        }
        else
        {
            supplier.SetAddress(null, command.UpdatedBy);
        }

        _suppliers.Update(supplier);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(supplier);
    }

    internal static SupplierDetailDto ToDto(Supplier supplier) =>
        new(supplier.Id, supplier.Name, supplier.TaxId,
            supplier.Phone, supplier.Email, supplier.Website,
            supplier.ContactName, supplier.Notes, supplier.IsActive,
            supplier.Address is null ? null : new SupplierAddressDto(
                supplier.Address.Street, supplier.Address.City,
                supplier.Address.PostalCode, supplier.Address.Country),
            supplier.CreatedAt, supplier.CreatedBy, supplier.UpdatedAt);
}
