using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.UpsertSupplierItemCode;

public sealed class UpsertSupplierItemCodeHandler
{
    private readonly ISupplierItemCodeRepository _codes;
    private readonly IUnitOfWork _uow;

    public UpsertSupplierItemCodeHandler(ISupplierItemCodeRepository codes, IUnitOfWork uow)
    {
        _codes = codes;
        _uow = uow;
    }

    public async Task<SupplierItemCodeDto> Handle(
        UpsertSupplierItemCodeCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await _codes.GetBySupplierAndItemAsync(
            command.SupplierId, command.ItemId, cancellationToken);

        if (existing is not null)
        {
            existing.Update(command.SupplierCode, command.Description, command.UpdatedBy);
            _codes.Update(existing);
        }
        else
        {
            existing = SupplierItemCode.Create(
                command.SupplierId, command.ItemId,
                command.SupplierCode, command.Description, command.UpdatedBy);
            await _codes.AddAsync(existing, cancellationToken);
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return new SupplierItemCodeDto(
            existing.Id, existing.SupplierId, existing.ItemId,
            existing.Item?.Code ?? "", existing.Item?.Name ?? "",
            existing.SupplierCode, existing.Description,
            existing.CreatedAt);
    }
}
