using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetAutomationPreview;

public sealed class GetAutomationPreviewHandler
{
    private readonly ISalesOrderRepository _orders;
    private readonly ISalesDeliveryNoteRepository _notes;

    public GetAutomationPreviewHandler(ISalesOrderRepository orders, ISalesDeliveryNoteRepository notes)
    {
        _orders = orders;
        _notes = notes;
    }

    public async Task<AutomationPreviewResult> Handle(CancellationToken cancellationToken = default)
    {
        var pendingOrders = await _orders.GetConfirmedPendingDeliveryAsync(cancellationToken);
        var postedNotes = await _notes.GetPostedWithoutInvoiceAsync(cancellationToken);

        var orderItems = pendingOrders.Select(o => new AutomationPreviewItem(
            o.Id, o.Number, o.Customer?.Name ?? string.Empty, o.Date,
            o.Status == SalesOrderStatus.Confirmed ? "Confirmado" : "Parcialmente entregado")).ToList();

        var noteItems = postedNotes.Select(n => new AutomationPreviewItem(
            n.Id, n.Number, n.Customer?.Name ?? string.Empty, n.Date,
            GetSalesDeliveryNoteByIdHandler.StatusLabel(n.Status))).ToList();

        return new AutomationPreviewResult(orderItems, noteItems);
    }
}
