using Debales.Application.Common;
using Debales.Application.CRM.Contacts.DTOs;
using Debales.Application.CRM.Customers;
using Debales.Domain.CRM.Contacts;

namespace Debales.Application.CRM.Contacts.Commands.AddContact;

public sealed class AddContactHandler
{
    private readonly IContactRepository _contacts;
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public AddContactHandler(IContactRepository contacts, ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _contacts = contacts;
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<ContactDto> Handle(AddContactCommand command, CancellationToken cancellationToken = default)
    {
        var customerExists = await _customers.GetByIdAsync(command.CustomerId, cancellationToken) is not null;
        if (!customerExists)
            throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        var contact = Contact.Create(
            command.CustomerId, command.FirstName, command.LastName,
            command.JobTitle, command.Email, command.Phone, command.CreatedBy);

        await _contacts.AddAsync(contact, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ContactDto(contact.Id, contact.CustomerId, contact.FirstName, contact.LastName,
            contact.FullName, contact.JobTitle, contact.Email, contact.Phone, contact.IsActive, contact.CreatedAt);
    }
}
