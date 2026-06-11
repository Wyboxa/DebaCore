using Debales.Application.Inventory.DTOs;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetInventoryCountById;

public sealed class GetInventoryCountByIdHandler
{
    private readonly IInventoryCountRepository _counts;

    public GetInventoryCountByIdHandler(IInventoryCountRepository counts) => _counts = counts;

    public async Task<InventoryCountDetailDto?> Handle(GetInventoryCountByIdQuery query, CancellationToken ct = default)
    {
        var count = await _counts.GetByIdAsync(query.Id, ct);
        return count is null ? null : ToDetail(count);
    }

    internal static InventoryCountDetailDto ToDetail(InventoryCount c) => new(
        c.Id, c.Number, c.Date,
        c.WarehouseId, c.Warehouse?.Name ?? string.Empty,
        c.Status.ToString(),
        c.Notes,
        c.Lines.Select(ToLineDto).ToList(),
        c.CreatedAt, c.CreatedBy, c.UpdatedAt, c.UpdatedBy);

    private static InventoryCountLineDto ToLineDto(InventoryCountLine l) => new(
        l.Id, l.ItemId, l.ItemCode, l.ItemName,
        l.SystemQuantity, l.CountedQuantity,
        l.Difference, l.IsCounted);
}
