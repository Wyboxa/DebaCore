using Debales.Application.Purchasing.DTOs;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseOrderById;

public sealed class GetPurchaseOrderByIdHandler
{
    private readonly IPurchaseOrderRepository _orders;

    public GetPurchaseOrderByIdHandler(IPurchaseOrderRepository orders) => _orders = orders;

    public async Task<PurchaseOrderDetailDto?> Handle(GetPurchaseOrderByIdQuery query, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(query.Id, cancellationToken);
        return order is null ? null : ToDto(order);
    }

    internal static PurchaseOrderDetailDto ToDto(PurchaseOrder order) => new(
        order.Id,
        order.Number,
        order.SupplierId,
        order.Supplier?.Name ?? string.Empty,
        order.Date,
        order.ExpectedReceiptDate,
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

    private static PurchaseOrderLineSummaryDto ToLineDto(PurchaseOrderLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.ReceivedQuantity, l.PendingQuantity,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(PurchaseOrderStatus s) => s switch
    {
        PurchaseOrderStatus.Draft => "Borrador",
        PurchaseOrderStatus.Confirmed => "Confirmado",
        PurchaseOrderStatus.PartiallyReceived => "Recibido parcialmente",
        PurchaseOrderStatus.Received => "Recibido",
        PurchaseOrderStatus.Cancelled => "Cancelado",
        _ => s.ToString()
    };
}
