using Debales.Application.Inventory.DTOs;
using Debales.Application.Inventory.Queries.GetWarehouseById;

namespace Debales.Application.Inventory.Queries.GetWarehouses;

public sealed class GetWarehousesHandler
{
    private readonly IWarehouseRepository _warehouses;

    public GetWarehousesHandler(IWarehouseRepository warehouses) => _warehouses = warehouses;

    public async Task<IReadOnlyList<WarehouseSummaryDto>> Handle(GetWarehousesQuery query, CancellationToken cancellationToken = default)
    {
        var list = query.ActiveOnly
            ? await _warehouses.GetAllActiveAsync(cancellationToken)
            : await _warehouses.GetAllAsync(cancellationToken);

        return list.Select(GetWarehouseByIdHandler.ToSummaryDto).ToList();
    }
}
