using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetInventoryCountById;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Commands.CreateInventoryCount;

public sealed class CreateInventoryCountHandler
{
    private readonly IInventoryCountRepository _counts;
    private readonly IWarehouseRepository _warehouses;
    private readonly IUnitOfWork _uow;

    public CreateInventoryCountHandler(
        IInventoryCountRepository counts, IWarehouseRepository warehouses, IUnitOfWork uow)
    {
        _counts = counts;
        _warehouses = warehouses;
        _uow = uow;
    }

    public async Task<InventoryCountDetailDto> Handle(CreateInventoryCountCommand command, CancellationToken ct = default)
    {
        _ = await _warehouses.GetByIdAsync(command.WarehouseId, ct)
            ?? throw new InvalidOperationException($"Almacén '{command.WarehouseId}' no encontrado.");

        var number = await _counts.GetNextNumberAsync(ct);
        var count = InventoryCount.Create(number, command.Date, command.WarehouseId, command.Notes, command.CreatedBy);
        await _counts.AddAsync(count, ct);
        await _uow.SaveChangesAsync(ct);

        var saved = await _counts.GetByIdAsync(count.Id, ct);
        return GetInventoryCountByIdHandler.ToDetail(saved!);
    }
}
