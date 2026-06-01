using Debales.Application.Inventory.DTOs;
using Debales.Domain.Inventory;

namespace Debales.Application.Inventory.Queries.GetStockBalance;

public sealed class GetStockBalanceHandler
{
    private readonly IStockBalanceRepository _balances;

    public GetStockBalanceHandler(IStockBalanceRepository balances) => _balances = balances;

    public async Task<IReadOnlyList<StockBalanceDto>> Handle(GetStockBalanceQuery query, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<StockBalance> balances;

        if (query.ItemId.HasValue)
            balances = await _balances.GetByItemAsync(query.ItemId.Value, cancellationToken);
        else if (query.WarehouseId.HasValue)
            balances = await _balances.GetByWarehouseAsync(query.WarehouseId.Value, cancellationToken);
        else
            balances = await _balances.GetAllWithStockAsync(cancellationToken);

        return balances.Select(ToDto).ToList();
    }

    private static StockBalanceDto ToDto(StockBalance b) => new(
        b.ItemId, b.Item?.Code ?? string.Empty, b.Item?.Name ?? string.Empty,
        b.WarehouseId, b.Warehouse?.Name ?? string.Empty,
        b.Quantity);
}
