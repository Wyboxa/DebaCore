using Debales.Application.Common;
using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetStockMovementById;

namespace Debales.Application.Inventory.Queries.GetStockMovements;

public sealed class GetStockMovementsHandler
{
    private readonly IStockMovementRepository _movements;

    public GetStockMovementsHandler(IStockMovementRepository movements) => _movements = movements;

    public async Task<PagedResult<StockMovementSummaryDto>> Handle(GetStockMovementsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _movements.SearchAsync(query.Search, query.ItemId, query.WarehouseId, query.Type, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(GetStockMovementByIdHandler.ToSummaryDto).ToList();
        return new PagedResult<StockMovementSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
