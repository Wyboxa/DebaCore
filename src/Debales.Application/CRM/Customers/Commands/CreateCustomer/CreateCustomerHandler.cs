using Debales.Application.Common;
using Debales.Application.CRM.Customers.DTOs;
using Debales.Domain.CRM.Customers;

namespace Debales.Application.CRM.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerHandler(ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerSummaryDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(command.TaxId) &&
            await _customers.ExistsByTaxIdAsync(command.TaxId, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un cliente con el NIF/CIF '{command.TaxId}'.");
        }

        var customer = Customer.Create(command.Name, command.Sector, command.TaxId, command.Phone, command.CreatedBy);

        await _customers.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CustomerSummaryDto(customer.Id, customer.Name, customer.Sector, customer.TaxId, customer.Phone, customer.IsActive, customer.CreatedAt);
    }
}
