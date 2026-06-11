using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetInventoryCountById;

namespace Debales.Application.Inventory.Commands.SetCountedQuantity;

public sealed class SetCountedQuantityHandler
{
    private readonly IInventoryCountRepository _counts;
    private readonly IUnitOfWork _uow;

    public SetCountedQuantityHandler(IInventoryCountRepository counts, IUnitOfWork uow)
    {
        _counts = counts;
        _uow = uow;
    }

    public async Task<InventoryCountDetailDto> Handle(SetCountedQuantityCommand command, CancellationToken ct = default)
    {
        var count = await _counts.GetByIdAsync(command.InventoryCountId, ct)
            ?? throw new InvalidOperationException("Recuento no encontrado.");

        count.SetCountedQuantity(command.ItemId, command.CountedQuantity, command.UpdatedBy);
        await _uow.SaveChangesAsync(ct);

        var saved = await _counts.GetByIdAsync(count.Id, ct);
        return GetInventoryCountByIdHandler.ToDetail(saved!);
    }
}
