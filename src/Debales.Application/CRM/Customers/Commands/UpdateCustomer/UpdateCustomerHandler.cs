using Debales.Application.Common;
using Debales.Application.CRM.Customers.DTOs;

namespace Debales.Application.CRM.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerHandler
{
    private readonly ICustomerRepository _customers;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerHandler(ICustomerRepository customers, IUnitOfWork unitOfWork)
    {
        _customers = customers;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDetailDto> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        var customer = await _customers.GetByIdAsync(command.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente '{command.CustomerId}' no encontrado.");

        customer.Update(command.Name, command.Sector, command.TaxId, command.Phone, command.Website, command.UpdatedBy);

        _customers.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CustomerDetailDto(
            customer.Id, customer.Name, customer.Sector, customer.TaxId,
            customer.Phone, customer.Website, customer.IsActive,
            customer.Address is null ? null : new AddressDto(customer.Address.Street, customer.Address.City, customer.Address.PostalCode, customer.Address.Country),
            customer.CreatedAt, customer.CreatedBy, customer.UpdatedAt);
    }
}
