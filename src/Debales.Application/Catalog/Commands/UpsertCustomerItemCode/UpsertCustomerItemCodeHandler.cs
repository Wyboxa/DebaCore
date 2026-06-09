using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.UpsertCustomerItemCode;

public sealed class UpsertCustomerItemCodeHandler
{
    private readonly ICustomerItemCodeRepository _codes;
    private readonly IUnitOfWork _uow;

    public UpsertCustomerItemCodeHandler(ICustomerItemCodeRepository codes, IUnitOfWork uow)
    {
        _codes = codes;
        _uow = uow;
    }

    public async Task<CustomerItemCodeDto> Handle(
        UpsertCustomerItemCodeCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await _codes.GetByCustomerAndItemAsync(
            command.CustomerId, command.ItemId, cancellationToken);

        if (existing is not null)
        {
            existing.Update(command.CustomerCode, command.Description, command.UpdatedBy);
            _codes.Update(existing);
        }
        else
        {
            existing = CustomerItemCode.Create(
                command.CustomerId, command.ItemId,
                command.CustomerCode, command.Description, command.UpdatedBy);
            await _codes.AddAsync(existing, cancellationToken);
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return new CustomerItemCodeDto(
            existing.Id, existing.CustomerId, existing.ItemId,
            existing.Item?.Code ?? "", existing.Item?.Name ?? "",
            existing.CustomerCode, existing.Description,
            existing.CreatedAt);
    }
}
