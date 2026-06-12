using Debales.Application.Suppliers.Contacts.Commands.AddSupplierContact;
using Debales.Application.Suppliers.Contacts.DTOs;

namespace Debales.Application.Suppliers.Contacts.Queries.GetContactsBySupplier;

public sealed record GetContactsBySupplierQuery(Guid SupplierId);

public sealed class GetContactsBySupplierHandler
{
    private readonly ISupplierContactRepository _contacts;

    public GetContactsBySupplierHandler(ISupplierContactRepository contacts) => _contacts = contacts;

    public async Task<IReadOnlyList<SupplierContactDto>> Handle(GetContactsBySupplierQuery query, CancellationToken cancellationToken = default)
    {
        var contacts = await _contacts.GetBySupplierIdAsync(query.SupplierId, cancellationToken);
        return contacts.Select(AddSupplierContactHandler.ToDto).ToList();
    }
}
