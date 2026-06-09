using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Application.Suppliers.DTOs;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers.Commands.UpdateSupplier;

public sealed class UpdateSupplierHandler
{
    private readonly ISupplierRepository _suppliers;
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IPaymentMethodRepository _paymentMethods;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSupplierHandler(ISupplierRepository suppliers, IPaymentTermRepository paymentTerms,
        IPaymentMethodRepository paymentMethods, IUnitOfWork unitOfWork)
    {
        _suppliers = suppliers;
        _paymentTerms = paymentTerms;
        _paymentMethods = paymentMethods;
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

        supplier.SetAccountCode(command.AccountCode, command.UpdatedBy);
        supplier.SetPaymentTerm(command.PaymentTermId, command.UpdatedBy);
        supplier.SetPaymentMethod(command.PaymentMethodId, command.UpdatedBy);

        _suppliers.Update(supplier);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        string? paymentTermName = null;
        if (supplier.PaymentTermId.HasValue)
        {
            var pt = await _paymentTerms.GetByIdAsync(supplier.PaymentTermId.Value, cancellationToken);
            paymentTermName = pt?.Name;
        }

        string? paymentMethodName = null;
        if (supplier.PaymentMethodId.HasValue)
        {
            var pm = await _paymentMethods.GetByIdAsync(supplier.PaymentMethodId.Value, cancellationToken);
            paymentMethodName = pm?.Name;
        }

        return ToDto(supplier, paymentTermName, paymentMethodName);
    }

    internal static SupplierDetailDto ToDto(Supplier supplier, string? paymentTermName = null, string? paymentMethodName = null) =>
        new(supplier.Id, supplier.Name, supplier.TaxId,
            supplier.Phone, supplier.Email, supplier.Website,
            supplier.ContactName, supplier.Notes, supplier.IsActive,
            supplier.Address is null ? null : new SupplierAddressDto(
                supplier.Address.Street, supplier.Address.City,
                supplier.Address.PostalCode, supplier.Address.Country),
            supplier.CreatedAt, supplier.CreatedBy, supplier.UpdatedAt,
            AccountCode: supplier.AccountCode,
            PaymentTermId: supplier.PaymentTermId,
            PaymentTermName: paymentTermName,
            PaymentMethodId: supplier.PaymentMethodId,
            PaymentMethodName: paymentMethodName);
}
