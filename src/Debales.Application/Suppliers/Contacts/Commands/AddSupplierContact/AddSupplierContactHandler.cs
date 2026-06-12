using Debales.Application.Common;
using Debales.Application.Suppliers.Contacts.DTOs;
using Debales.Domain.Suppliers;

namespace Debales.Application.Suppliers.Contacts.Commands.AddSupplierContact;

public sealed class AddSupplierContactHandler
{
    private readonly ISupplierContactRepository _contacts;
    private readonly ISupplierRepository _suppliers;
    private readonly IUnitOfWork _unitOfWork;

    public AddSupplierContactHandler(ISupplierContactRepository contacts, ISupplierRepository suppliers, IUnitOfWork unitOfWork)
    {
        _contacts = contacts;
        _suppliers = suppliers;
        _unitOfWork = unitOfWork;
    }

    public async Task<SupplierContactDto> Handle(AddSupplierContactCommand command, CancellationToken cancellationToken = default)
    {
        var supplierExists = await _suppliers.GetByIdAsync(command.SupplierId, cancellationToken) is not null;
        if (!supplierExists)
            throw new KeyNotFoundException($"Proveedor '{command.SupplierId}' no encontrado.");

        var contact = SupplierContact.Create(
            command.SupplierId, command.FirstName, command.LastName,
            command.JobTitle, command.Email, command.Phone, command.CreatedBy);

        await _contacts.AddAsync(contact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(contact);
    }

    internal static SupplierContactDto ToDto(SupplierContact c) =>
        new(c.Id, c.SupplierId, c.FirstName, c.LastName,
            c.FullName, c.JobTitle, c.Email, c.Phone, c.IsActive, c.CreatedAt);
}
