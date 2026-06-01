using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetStockMovementById;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.CreateStockMovement;

public sealed class CreateStockMovementHandler
{
    private readonly IStockMovementRepository _movements;
    private readonly IStockBalanceRepository _balances;
    private readonly IItemRepository _items;
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public CreateStockMovementHandler(
        IStockMovementRepository movements, IStockBalanceRepository balances,
        IItemRepository items, IWarehouseRepository warehouses, IUnitOfWork uow)
    {
        _movements = movements;
        _balances = balances;
        _items = items;
        _warehouses = warehouses;
        _uow = uow;
    }

    public async Task<StockMovementDetailDto> Handle(CreateStockMovementCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Quantity == 0)
            throw new ArgumentException("La cantidad no puede ser cero.");

        var item = await _items.GetByIdAsync(command.ItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Artículo '{command.ItemId}' no encontrado.");

        var warehouse = await _warehouses.GetByIdAsync(command.WarehouseId, cancellationToken)
            ?? throw new InvalidOperationException($"Almacén '{command.WarehouseId}' no encontrado.");

        // Signed quantity: Out and negative adjustments are stored as-is (caller passes negative)
        // In: positive, Out: negative, Adjustment: signed
        decimal signedQty = command.Type == StockMovementType.Out
            ? -Math.Abs(command.Quantity)
            : command.Quantity;

        var number = await _movements.GetNextNumberAsync(cancellationToken);
        var movement = StockMovement.Create(
            number, command.Type,
            item.Id, item.Code, item.Name,
            command.WarehouseId, command.LocationId,
            command.Date, signedQty,
            command.Reference, command.Notes, command.CreatedBy);

        await _movements.AddAsync(movement, cancellationToken);

        // Update stock balance (upsert)
        var balance = await _balances.GetAsync(item.Id, command.WarehouseId, cancellationToken);
        if (balance is null)
        {
            balance = StockBalance.Create(item.Id, command.WarehouseId);
            balance.Apply(signedQty);
            await _balances.AddAsync(balance, cancellationToken);
        }
        else
        {
            balance.Apply(signedQty);
        }

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _movements.GetByIdAsync(movement.Id, cancellationToken);
        return GetStockMovementByIdHandler.ToDetailDto(saved!);
    }
}
