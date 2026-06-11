using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetInventoryCounts;

public sealed class GetInventoryCountsHandler
{
    private readonly IInventoryCountRepository _counts;

    public GetInventoryCountsHandler(IInventoryCountRepository counts) => _counts = counts;

    public async Task<PagedResult<InventoryCountSummaryDto>> Handle(GetInventoryCountsQuery query, CancellationToken ct = default)
    {
        var result = await _counts.SearchAsync(query.WarehouseId, query.Status, query.Page, query.PageSize, ct);
        return new PagedResult<InventoryCountSummaryDto>(
            result.Items.Select(ToSummary).ToList(),
            result.TotalCount, result.Page, result.PageSize);
    }

    private static InventoryCountSummaryDto ToSummary(InventoryCount c) => new(
        c.Id, c.Number, c.Date,
        c.WarehouseId, c.Warehouse?.Name ?? string.Empty,
        c.Status.ToString(),
        c.Lines.Count,
        c.Lines.Count(l => l.IsCounted),
        c.CreatedAt, c.CreatedBy);
}
