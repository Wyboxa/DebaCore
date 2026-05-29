using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesOrderById;

public sealed class GetSalesOrderByIdHandler
{
    private readonly ISalesOrderRepository _orders;

    public GetSalesOrderByIdHandler(ISalesOrderRepository orders) => _orders = orders;

    public async Task<SalesOrderDetailDto?> Handle(GetSalesOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(query.Id, cancellationToken);
        return order is null ? null : ToDto(order);
    }

    internal static SalesOrderDetailDto ToDto(SalesOrder order) => new(
        order.Id,
        order.Number,
        order.CustomerId,
        order.Customer?.Name ?? string.Empty,
        order.Date,
        order.RequestedDeliveryDate,
        order.Status,
        StatusLabel(order.Status),
        order.Notes,
        order.Lines.Select(ToLineDto).ToList(),
        order.Subtotal,
        order.TaxAmount,
        order.Total,
        order.CreatedAt,
        order.CreatedBy,
        order.UpdatedAt,
        order.UpdatedBy);

    private static SalesOrderLineSummaryDto ToLineDto(SalesOrderLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.DeliveredQuantity, l.PendingQuantity,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(SalesOrderStatus s) => s switch
    {
        SalesOrderStatus.Draft => "Borrador",
        SalesOrderStatus.Confirmed => "Confirmado",
        SalesOrderStatus.PartiallyDelivered => "Parcialmente entregado",
        SalesOrderStatus.Delivered => "Entregado",
        SalesOrderStatus.Cancelled => "Cancelado",
        _ => s.ToString()
    };
}
