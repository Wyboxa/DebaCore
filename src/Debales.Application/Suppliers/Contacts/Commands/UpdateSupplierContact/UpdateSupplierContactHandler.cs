using Debales.Application.Common;
using Debales.Application.Suppliers.Contacts.Commands.AddSupplierContact;
using Debales.Application.Suppliers.Contacts.DTOs;

namespace Debales.Application.Suppliers.Contacts.Commands.UpdateSupplierContact;

public sealed class UpdateSupplierContactHandler
{
    private readonly ISupplierContactRepository _contacts;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSupplierContactHandler(ISupplierContactRepository contacts, IUnitOfWork unitOfWork)
    {
        _contacts = contacts;
        _unitOfWork = unitOfWork;
    }

    public async Task<SupplierContactDto> Handle(UpdateSupplierContactCommand command, CancellationToken cancellationToken = default)
    {
        var contact = await _contacts.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Contacto '{command.Id}' no encontrado.");

        contact.Update(command.FirstName, command.LastName, command.JobTitle,
            command.Email, command.Phone, command.UpdatedBy);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AddSupplierContactHandler.ToDto(contact);
    }
}
