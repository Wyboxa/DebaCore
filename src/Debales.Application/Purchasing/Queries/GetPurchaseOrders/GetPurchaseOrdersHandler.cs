using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseOrderById;

namespace Debales.Application.Purchasing.Queries.GetPurchaseOrders;

public sealed class GetPurchaseOrdersHandler
{
    private readonly IPurchaseOrderRepository _orders;

    public GetPurchaseOrdersHandler(IPurchaseOrderRepository orders) => _orders = orders;

    public async Task<PagedResult<PurchaseOrderSummaryDto>> Handle(GetPurchaseOrdersQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _orders.SearchAsync(
            query.Search, query.SupplierId, query.Status,
            query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items.Select(o => new PurchaseOrderSummaryDto(
            o.Id, o.Number,
            o.SupplierId, o.Supplier?.Name ?? string.Empty,
            o.Date, o.ExpectedReceiptDate,
            o.Status, GetPurchaseOrderByIdHandler.StatusLabel(o.Status),
            o.Total)).ToList();

        return new PagedResult<PurchaseOrderSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
