using Debales.Application.Inventory.DTOs;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetWarehouseById;

public sealed class GetWarehouseByIdHandler
{
    private readonly IWarehouseRepository _warehouses;

    public GetWarehouseByIdHandler(IWarehouseRepository warehouses) => _warehouses = warehouses;

    public async Task<WarehouseDetailDto?> Handle(GetWarehouseByIdQuery query, CancellationToken cancellationToken = default)
    {
        var w = await _warehouses.GetByIdAsync(query.Id, cancellationToken);
        return w is null ? null : ToDetailDto(w);
    }

    internal static WarehouseDetailDto ToDetailDto(Warehouse w) => new(
        w.Id, w.Code, w.Name, w.Description, w.IsActive,
        w.Locations.Select(l => new WarehouseLocationDto(l.Id, l.WarehouseId, l.Code, l.Description, l.IsActive)).ToList(),
        w.CreatedAt, w.CreatedBy);

    internal static WarehouseSummaryDto ToSummaryDto(Warehouse w) => new(w.Id, w.Code, w.Name, w.Description, w.IsActive);
}
