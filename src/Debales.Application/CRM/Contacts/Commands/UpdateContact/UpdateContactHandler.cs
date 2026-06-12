using Debales.Application.Common;
using Debales.Application.CRM.Contacts.DTOs;

namespace Debales.Application.CRM.Contacts.Commands.UpdateContact;

public sealed class UpdateContactHandler
{
    private readonly IContactRepository _contacts;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateContactHandler(IContactRepository contacts, IUnitOfWork unitOfWork)
    {
        _contacts = contacts;
        _unitOfWork = unitOfWork;
    }

    public async Task<ContactDto> Handle(UpdateContactCommand command, CancellationToken cancellationToken = default)
    {
        var contact = await _contacts.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Contacto '{command.Id}' no encontrado.");

        contact.Update(command.FirstName, command.LastName, command.JobTitle,
            command.Email, command.Phone, command.UpdatedBy);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ContactDto(contact.Id, contact.CustomerId, contact.FirstName, contact.LastName,
            contact.FullName, contact.JobTitle, contact.Email, contact.Phone, contact.IsActive, contact.CreatedAt);
    }
}
