using Debales.Application.Catalog;
using Debales.Application.CRM.Customers.DTOs;
using Debales.Application.Sales;

namespace Debales.Application.CRM.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IPriceListRepository _priceLists;
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IPaymentMethodRepository _paymentMethods;

    public GetCustomerByIdHandler(ICustomerRepository customers, IPriceListRepository priceLists,
        IPaymentTermRepository paymentTerms, IPaymentMethodRepository paymentMethods)
    {
        _customers = customers;
        _priceLists = priceLists;
        _paymentTerms = paymentTerms;
        _paymentMethods = paymentMethods;
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

        string? paymentTermName = null;
        if (customer.PaymentTermId.HasValue)
        {
            var pt = await _paymentTerms.GetByIdAsync(customer.PaymentTermId.Value, cancellationToken);
            paymentTermName = pt?.Name;
        }

        string? paymentMethodName = null;
        if (customer.PaymentMethodId.HasValue)
        {
            var pm = await _paymentMethods.GetByIdAsync(customer.PaymentMethodId.Value, cancellationToken);
            paymentMethodName = pm?.Name;
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
            PriceListName: priceListName,
            PaymentTermId: customer.PaymentTermId,
            PaymentTermName: paymentTermName,
            PaymentMethodId: customer.PaymentMethodId,
            PaymentMethodName: paymentMethodName);
    }
}
