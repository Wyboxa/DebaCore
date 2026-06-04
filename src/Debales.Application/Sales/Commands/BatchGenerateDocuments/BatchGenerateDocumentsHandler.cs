using Debales.Application.Sales.Commands.GenerateDeliveryNoteFromOrder;
using Debales.Application.Sales.Commands.GenerateInvoiceFromDeliveryNote;

namespace Debales.Application.Sales.Commands.BatchGenerateDocuments;

public sealed class BatchGenerateDocumentsHandler
{
    private readonly ISalesOrderRepository _orders;
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly GenerateDeliveryNoteFromOrderHandler _generateNoteHandler;
    private readonly GenerateInvoiceFromDeliveryNoteHandler _generateInvoiceHandler;

    public BatchGenerateDocumentsHandler(
        ISalesOrderRepository orders,
        ISalesDeliveryNoteRepository notes,
        GenerateDeliveryNoteFromOrderHandler generateNoteHandler,
        GenerateInvoiceFromDeliveryNoteHandler generateInvoiceHandler)
    {
        _orders = orders;
        _notes = notes;
        _generateNoteHandler = generateNoteHandler;
        _generateInvoiceHandler = generateInvoiceHandler;
    }

    public async Task<BatchGenerateDocumentsResult> Handle(
        BatchGenerateDocumentsCommand command,
        CancellationToken cancellationToken = default)
    {
        return command.Mode switch
        {
            BatchMode.OrdersToDeliveryNotes => await ProcessOrdersToNotesAsync(command.CreatedBy, cancellationToken),
            BatchMode.DeliveryNotesToInvoices => await ProcessNotesToInvoicesAsync(command.CreatedBy, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(command.Mode))
        };
    }

    private async Task<BatchGenerateDocumentsResult> ProcessOrdersToNotesAsync(string createdBy, CancellationToken ct)
    {
        var orders = await _orders.GetConfirmedPendingDeliveryAsync(ct);
        var results = new List<BatchItemResult>();

        foreach (var order in orders)
        {
            try
            {
                var dto = await _generateNoteHandler.Handle(
                    new GenerateDeliveryNoteFromOrderCommand(order.Id, createdBy), ct);
                results.Add(new BatchItemResult(order.Id, order.Number, dto.Number, true, null));
            }
            catch (Exception ex)
            {
                results.Add(new BatchItemResult(order.Id, order.Number, null, false, ex.Message));
            }
        }

        return new BatchGenerateDocumentsResult(
            BatchMode.OrdersToDeliveryNotes,
            orders.Count,
            results.Count(r => r.Success),
            results.Count(r => !r.Success),
            results);
    }

    private async Task<BatchGenerateDocumentsResult> ProcessNotesToInvoicesAsync(string createdBy, CancellationToken ct)
    {
        var notes = await _notes.GetPostedWithoutInvoiceAsync(ct);
        var results = new List<BatchItemResult>();
        var dueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

        foreach (var note in notes)
        {
            try
            {
                var dto = await _generateInvoiceHandler.Handle(
                    new GenerateInvoiceFromDeliveryNoteCommand(note.Id, dueDate, createdBy), ct);
                results.Add(new BatchItemResult(note.Id, note.Number, dto.Number, true, null));
            }
            catch (Exception ex)
            {
                results.Add(new BatchItemResult(note.Id, note.Number, null, false, ex.Message));
            }
        }

        return new BatchGenerateDocumentsResult(
            BatchMode.DeliveryNotesToInvoices,
            notes.Count,
            results.Count(r => r.Success),
            results.Count(r => !r.Success),
            results);
    }
}
