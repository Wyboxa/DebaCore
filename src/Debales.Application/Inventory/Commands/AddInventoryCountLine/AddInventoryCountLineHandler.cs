using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetInventoryCountById;

namespace Debales.Application.Inventory.Commands.AddInventoryCountLine;

public sealed class AddInventoryCountLineHandler
{
    private readonly IInventoryCountRepository _counts;
    private readonly IStockBalanceRepository _balances;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public AddInventoryCountLineHandler(
        IInventoryCountRepository counts, IStockBalanceRepository balances,
        IItemRepository items, IUnitOfWork uow)
    {
        _counts = counts;
        _balances = balances;
        _items = items;
        _uow = uow;
    }

    public async Task<InventoryCountDetailDto> Handle(AddInventoryCountLineCommand command, CancellationToken ct = default)
    {
        var count = await _counts.GetByIdAsync(command.InventoryCountId, ct)
            ?? throw new InvalidOperationException("Recuento no encontrado.");

        var item = await _items.GetByIdAsync(command.ItemId, ct)
            ?? throw new InvalidOperationException("Artículo no encontrado.");

        var balance = await _balances.GetAsync(item.Id, count.WarehouseId, ct);
        var systemQty = balance?.Quantity ?? 0m;

        count.AddLine(item.Id, item.Code, item.Name, systemQty, command.CreatedBy);
        await _uow.SaveChangesAsync(ct);

        var saved = await _counts.GetByIdAsync(count.Id, ct);
        return GetInventoryCountByIdHandler.ToDetail(saved!);
    }
}
