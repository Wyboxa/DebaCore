using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetStockMovementById;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.AdjustStock;

public sealed class AdjustStockHandler
{
    private readonly IStockMovementRepository _movements;
    private readonly IStockBalanceRepository _balances;
    private readonly IItemRepository _items;
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public AdjustStockHandler(
        IStockMovementRepository movements, IStockBalanceRepository balances,
        IItemRepository items, IWarehouseRepository warehouses, IUnitOfWork uow)
    {
        _movements = movements;
        _balances = balances;
        _items = items;
        _warehouses = warehouses;
        _uow = uow;
    }

    public async Task<StockMovementDetailDto> Handle(AdjustStockCommand command, CancellationToken ct = default)
    {
        var item = await _items.GetByIdAsync(command.ItemId, ct)
            ?? throw new InvalidOperationException($"Artículo '{command.ItemId}' no encontrado.");

        var warehouse = await _warehouses.GetByIdAsync(command.WarehouseId, ct)
            ?? throw new InvalidOperationException($"Almacén '{command.WarehouseId}' no encontrado.");

        var balance = await _balances.GetAsync(command.ItemId, command.WarehouseId, ct);
        var currentQty = balance?.Quantity ?? 0m;
        var delta = command.TargetQuantity - currentQty;

        if (delta == 0)
            throw new InvalidOperationException("La cantidad objetivo es igual al stock actual. No hay diferencia que ajustar.");

        var number = await _movements.GetNextNumberAsync(ct);
        var movement = StockMovement.Create(
            number, StockMovementType.Adjustment,
            item.Id, item.Code, item.Name,
            command.WarehouseId, null,
            DateOnly.FromDateTime(DateTime.Today),
            delta,
            command.Reason, null, command.CreatedBy);

        await _movements.AddAsync(movement, ct);

        if (balance is null)
        {
            balance = StockBalance.Create(item.Id, command.WarehouseId);
            balance.Apply(delta);
            await _balances.AddAsync(balance, ct);
        }
        else
        {
            balance.Apply(delta);
        }

        await _uow.SaveChangesAsync(ct);

        var saved = await _movements.GetByIdAsync(movement.Id, ct);
        return GetStockMovementByIdHandler.ToDetailDto(saved!);
    }
}
