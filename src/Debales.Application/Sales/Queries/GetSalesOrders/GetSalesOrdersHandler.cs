using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesOrderById;

namespace Debales.Application.Sales.Queries.GetSalesOrders;

public sealed class GetSalesOrdersHandler
{
    private readonly ISalesOrderRepository _orders;

    public GetSalesOrdersHandler(ISalesOrderRepository orders) => _orders = orders;

    public async Task<PagedResult<SalesOrderSummaryDto>> Handle(GetSalesOrdersQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _orders.SearchAsync(
            query.Search, query.CustomerId, query.Status,
            query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items.Select(o => new SalesOrderSummaryDto(
            o.Id, o.Number,
            o.CustomerId, o.Customer?.Name ?? string.Empty,
            o.Date, o.RequestedDeliveryDate,
            o.Status, GetSalesOrderByIdHandler.StatusLabel(o.Status),
            o.Total)).ToList();

        return new PagedResult<SalesOrderSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
