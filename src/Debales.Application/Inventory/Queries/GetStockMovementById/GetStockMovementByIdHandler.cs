using Debales.Application.Inventory.DTOs;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetStockMovementById;

public sealed class GetStockMovementByIdHandler
{
    private readonly IStockMovementRepository _movements;

    public GetStockMovementByIdHandler(IStockMovementRepository movements) => _movements = movements;

    public async Task<StockMovementDetailDto?> Handle(GetStockMovementByIdQuery query, CancellationToken cancellationToken = default)
    {
        var m = await _movements.GetByIdAsync(query.Id, cancellationToken);
        return m is null ? null : ToDetailDto(m);
    }

    internal static StockMovementDetailDto ToDetailDto(StockMovement m) => new(
        m.Id, m.Number, m.Type, TypeLabel(m.Type),
        m.ItemId, m.ItemCode, m.ItemName,
        m.WarehouseId, m.Warehouse?.Name ?? string.Empty,
        m.LocationId, m.Location?.Code,
        m.Date, m.Quantity,
        m.Reference, m.Notes,
        m.CreatedAt, m.CreatedBy);

    internal static StockMovementSummaryDto ToSummaryDto(StockMovement m) => new(
        m.Id, m.Number, m.Type, TypeLabel(m.Type),
        m.ItemId, m.ItemCode, m.ItemName,
        m.WarehouseId, m.Warehouse?.Name ?? string.Empty,
        m.Location?.Code,
        m.Date, m.Quantity, m.Reference);

    internal static string TypeLabel(StockMovementType t) => t switch
    {
        StockMovementType.In => "Entrada",
        StockMovementType.Out => "Salida",
        StockMovementType.Adjustment => "Ajuste",
        _ => t.ToString()
    };
}
