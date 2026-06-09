using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.GenerateInvoiceFromPurchaseDeliveryNote;

public sealed class GenerateInvoiceFromPurchaseDeliveryNoteHandler
{
    private readonly IPurchaseDeliveryNoteRepository _notes;
    private readonly IPurchaseOrderRepository _orders;
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public GenerateInvoiceFromPurchaseDeliveryNoteHandler(
        IPurchaseDeliveryNoteRepository notes,
        IPurchaseOrderRepository orders,
        IPurchaseInvoiceRepository invoices,
        IItemRepository items,
        INumberSeriesRepository series,
        IUnitOfWork uow)
    {
        _notes = notes;
        _orders = orders;
        _invoices = invoices;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<PurchaseInvoiceDetailDto> Handle(
        GenerateInvoiceFromPurchaseDeliveryNoteCommand command,
        CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.PurchaseDeliveryNoteId, cancellationToken)
            ?? throw new InvalidOperationException("Albarán de compra no encontrado.");

        if (note.Status != PurchaseDeliveryNoteStatus.Posted)
            throw new InvalidOperationException("Solo se puede facturar un albarán recepcionado (Posted).");

        var existing = await _invoices.GetByPurchaseDeliveryNoteIdAsync(note.Id, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe la factura {existing.Number} para este albarán.");

        PurchaseOrder? order = null;
        if (note.PurchaseOrderId.HasValue)
            order = await _orders.GetByIdAsync(note.PurchaseOrderId.Value, cancellationToken);

        var serie = await _series.GetByCodeAsync("FC", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'FC' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);
        var today = DateOnly.FromDateTime(DateTime.Today);

        var invoice = PurchaseInvoice.Create(
            number, null,
            note.SupplierId,
            note.Id,
            today,
            command.DueDate,
            $"Factura de albarán {note.Number}",
            command.CreatedBy);

        foreach (var noteLine in note.Lines)
        {
            var orderLine = order?.Lines.FirstOrDefault(l => l.Id == noteLine.PurchaseOrderLineId);
            decimal unitPrice;
            decimal taxRate;

            if (orderLine is not null)
            {
                unitPrice = orderLine.UnitPrice;
                taxRate = orderLine.TaxRate;
            }
            else
            {
                var item = await _items.GetByIdAsync(noteLine.ItemId, cancellationToken);
                unitPrice = item?.PurchasePrice ?? 0m;
                taxRate = 21m;
            }

            invoice.AddLine(
                noteLine.ItemId, noteLine.ItemCode, noteLine.ItemName,
                noteLine.Description,
                noteLine.Quantity, unitPrice, taxRate);
        }

        await _invoices.AddAsync(invoice, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetPurchaseInvoiceByIdHandler.ToDto(saved!);
    }
}
