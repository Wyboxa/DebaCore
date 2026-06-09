using Debales.Application.Catalog;
using Debales.Application.CRM.Customers.DTOs;

namespace Debales.Application.CRM.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IPriceListRepository _priceLists;

    public GetCustomerByIdHandler(ICustomerRepository customers, IPriceListRepository priceLists)
    {
        _customers = customers;
        _priceLists = priceLists;
    }

    public async Task<CustomerDetailDto?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var customer = await _customers.GetByIdWithDetailsAsync(query.CustomerId, cancellationToken);

        if (customer is null) return null;

        string? priceListName = null;
        if (customer.PriceListId.HasValue)
        {
            var pl = await _priceLists.GetByIdAsync(customer.PriceListId.Value, cancellationToken);
            priceListName = pl?.Name;
        }

        return new CustomerDetailDto(
            customer.Id, customer.Name, customer.Sector, customer.TaxId,
            customer.Phone, customer.Email, customer.Website, customer.IsActive,
            customer.Address is null ? null : new AddressDto(
                customer.Address.Street, customer.Address.City,
                customer.Address.PostalCode, customer.Address.Country),
            customer.CreatedAt, customer.CreatedBy, customer.UpdatedAt,
            AccountCode: customer.AccountCode,
            PriceListId: customer.PriceListId,
            PriceListName: priceListName);
    }
}
