using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.CRM.Customers.DTOs;
using Debales.Application.Sales;
using Debales.Domain.CRM.Customers;

namespace Debales.Application.CRM.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IPriceListRepository _priceLists;
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IPaymentMethodRepository _paymentMethods;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler(ICustomerRepository customers, IPriceListRepository priceLists,
        IPaymentTermRepository paymentTerms, IPaymentMethodRepository paymentMethods, IUnitOfWork unitOfWork)
    {
        _customers = customers;
        _priceLists = priceLists;
        _paymentTerms = paymentTerms;
        _paymentMethods = paymentMethods;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDetailDto> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customer = await _customers.GetByIdAsync(command.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        customer.Update(
            command.Name, command.Sector, command.TaxId,
            command.Phone, command.Email, command.Website, command.UpdatedBy);

        if (!string.IsNullOrWhiteSpace(command.AddressCity))
        {
            var address = Address.Create(
                command.AddressStreet ?? string.Empty,
                command.AddressCity,
                command.AddressPostalCode ?? string.Empty,
                command.AddressCountry ?? string.Empty);
            customer.SetAddress(address, command.UpdatedBy);
        }
        else
        {
            customer.SetAddress(null, command.UpdatedBy);
        }

        customer.SetAccountCode(command.AccountCode, command.UpdatedBy);
        customer.SetPriceList(command.PriceListId, command.UpdatedBy);
        customer.SetPaymentTerm(command.PaymentTermId, command.UpdatedBy);
        customer.SetPaymentMethod(command.PaymentMethodId, command.UpdatedBy);

        _customers.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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

        return ToDto(customer, priceListName, paymentTermName, paymentMethodName);
    }

    internal static CustomerDetailDto ToDto(Customer customer, string? priceListName = null, string? paymentTermName = null, string? paymentMethodName = null) =>
        new(customer.Id, customer.Name, customer.Sector, customer.TaxId,
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
