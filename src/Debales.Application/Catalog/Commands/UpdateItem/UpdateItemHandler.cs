using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.UpdateItem;

public sealed class UpdateItemHandler
{
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public UpdateItemHandler(IItemRepository items, IUnitOfWork uow)
    {
        _items = items;
        _uow = uow;
    }

    public async Task<ItemDetailDto> Handle(UpdateItemCommand command, CancellationToken cancellationToken = default)
    {
        var item = await _items.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Artículo no encontrado.");

        if (await _items.ExistsByCodeAsync(command.Code, command.Id, cancellationToken))
            throw new InvalidOperationException($"Ya existe otro artículo con el código '{command.Code}'.");

        item.Update(
            command.Code, command.Name, command.Description,
            command.IsService, command.UnitOfMeasureId, command.FamilyId, command.TaxTypeId,
            command.SalePrice, command.PurchasePrice, command.UpdatedBy);

        _items.Update(item);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _items.GetByIdAsync(item.Id, cancellationToken);
        return ToDto(saved!);
    }

    internal static ItemDetailDto ToDto(Item item) => new(
        item.Id,
        item.Code,
        item.Name,
        item.Description,
        item.IsService,
        item.SalePrice,
        item.PurchasePrice,
        item.IsActive,
        item.FamilyId,
        item.Family?.Name,
        item.UnitOfMeasureId,
        item.UnitOfMeasure?.Name ?? string.Empty,
        item.TaxTypeId,
        item.TaxType?.Name,
        item.TaxType?.Rate,
        item.CreatedAt,
        item.CreatedBy,
        item.UpdatedAt,
        item.UpdatedBy);
}
