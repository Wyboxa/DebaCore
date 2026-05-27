using Debales.Application.CRM.Customers.DTOs;

namespace Debales.Application.CRM.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdHandler
{
    private readonly ICustomerRepository _customers;

    public GetCustomerByIdHandler(ICustomerRepository customers) => _customers = customers;

    public async Task<CustomerDetailDto?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var customer = await _customers.GetByIdWithDetailsAsync(query.CustomerId, cancellationToken);

        if (customer is null) return null;

        return new CustomerDetailDto(
            customer.Id, customer.Name, customer.Sector, customer.TaxId,
            customer.Phone, customer.Website, customer.IsActive,
            customer.Address is null ? null : new AddressDto(customer.Address.Street, customer.Address.City, customer.Address.PostalCode, customer.Address.Country),
            customer.CreatedAt, customer.CreatedBy, customer.UpdatedAt);
    }
}
