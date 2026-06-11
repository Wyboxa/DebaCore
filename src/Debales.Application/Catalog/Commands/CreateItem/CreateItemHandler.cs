using Debales.Application.Catalog.Commands.UpdateItem;
using Debales.Application.Catalog.DTOs;
using Debales.Application.Common;
using Debales.Domain.Catalog;

namespace Debales.Application.Catalog.Commands.CreateItem;

public sealed class CreateItemHandler
{
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public CreateItemHandler(IItemRepository items, IUnitOfWork uow)
    {
        _items = items;
        _uow = uow;
    }

    public async Task<ItemDetailDto> Handle(CreateItemCommand command, CancellationToken cancellationToken = default)
    {
        if (await _items.ExistsByCodeAsync(command.Code, cancellationToken))
            throw new InvalidOperationException($"Ya existe un artículo con el código '{command.Code}'.");

        var item = Item.Create(
            command.Code, command.Name, command.Description,
            command.IsService, command.UnitOfMeasureId, command.FamilyId, command.TaxTypeId,
            command.SalePrice, command.PurchasePrice, command.CreatedBy, command.MinimumStock);

        await _items.AddAsync(item, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _items.GetByIdAsync(item.Id, cancellationToken);
        return UpdateItemHandler.ToDto(saved!);
    }
}
