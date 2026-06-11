using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetInventoryCountById;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.CloseInventoryCount;

public sealed class CloseInventoryCountHandler
{
    private readonly IInventoryCountRepository _counts;
    private readonly IStockMovementRepository _movements;
    private readonly IStockBalanceRepository _balances;
    private readonly IUnitOfWork _uow;

    public CloseInventoryCountHandler(
        IInventoryCountRepository counts, IStockMovementRepository movements,
        IStockBalanceRepository balances, IUnitOfWork uow)
    {
        _counts = counts;
        _movements = movements;
        _balances = balances;
        _uow = uow;
    }

    public async Task<InventoryCountDetailDto> Handle(CloseInventoryCountCommand command, CancellationToken ct = default)
    {
        var count = await _counts.GetByIdAsync(command.InventoryCountId, ct)
            ?? throw new InvalidOperationException("Recuento no encontrado.");

        foreach (var line in count.Lines.Where(l => l.IsCounted && l.Difference != 0))
        {
            var balance = await _balances.GetAsync(line.ItemId, count.WarehouseId, ct);
            var number = await _movements.GetNextNumberAsync(ct);

            var movement = StockMovement.Create(
                number, StockMovementType.Adjustment,
                line.ItemId, line.ItemCode, line.ItemName,
                count.WarehouseId, null,
                count.Date,
                line.Difference,
                $"Recuento {count.Number}", null, command.UpdatedBy);

            await _movements.AddAsync(movement, ct);

            if (balance is null)
            {
                balance = StockBalance.Create(line.ItemId, count.WarehouseId);
                balance.Apply(line.Difference);
                await _balances.AddAsync(balance, ct);
            }
            else
            {
                balance.Apply(line.Difference);
            }
        }

        count.Close(command.UpdatedBy);
        await _uow.SaveChangesAsync(ct);

        var saved = await _counts.GetByIdAsync(count.Id, ct);
        return GetInventoryCountByIdHandler.ToDetail(saved!);
    }
}
