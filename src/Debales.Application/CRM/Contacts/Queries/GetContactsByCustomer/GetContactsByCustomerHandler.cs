using Debales.Application.CRM.Contacts.DTOs;

namespace Debales.Application.CRM.Contacts.Queries.GetContactsByCustomer;

public sealed record GetContactsByCustomerQuery(Guid CustomerId);

public sealed class GetContactsByCustomerHandler
{
    private readonly IContactRepository _contacts;

    public GetContactsByCustomerHandler(IContactRepository contacts) => _contacts = contacts;

    public async Task<IReadOnlyList<ContactDto>> Handle(GetContactsByCustomerQuery query, CancellationToken cancellationToken = default)
    {
        var contacts = await _contacts.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

        return contacts
            .Select(c => new ContactDto(c.Id, c.CustomerId, c.FirstName, c.LastName,
                c.FullName, c.JobTitle, c.Email, c.Phone, c.IsActive, c.CreatedAt))
            .ToList();
    }
}
