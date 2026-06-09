using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.GenerateInvoiceFromDeliveryNote;

public sealed class GenerateInvoiceFromDeliveryNoteHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly ISalesOrderRepository _orders;
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public GenerateInvoiceFromDeliveryNoteHandler(
        ISalesDeliveryNoteRepository notes,
        ISalesOrderRepository orders,
        ISalesInvoiceRepository invoices,
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

    public async Task<SalesInvoiceDetailDto> Handle(
        GenerateInvoiceFromDeliveryNoteCommand command,
        CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.SalesDeliveryNoteId, cancellationToken)
            ?? throw new InvalidOperationException("Albarán de venta no encontrado.");

        if (note.Status != SalesDeliveryNoteStatus.Posted)
            throw new InvalidOperationException("Solo se puede facturar un albarán emitido (Posted).");

        var existing = await _invoices.GetBySalesDeliveryNoteIdAsync(note.Id, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe la factura {existing.Number} para este albarán.");

        // Carga el pedido origen para recuperar precios de líneas
        SalesOrder? order = null;
        if (note.SalesOrderId.HasValue)
            order = await _orders.GetByIdAsync(note.SalesOrderId.Value, cancellationToken);

        var serie = await _series.GetByCodeAsync("FV", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'FV' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (command.DueDate < today)
            throw new ArgumentException("La fecha de vencimiento no puede ser anterior a hoy.");

        var invoice = SalesInvoice.Create(
            number,
            note.CustomerId,
            note.Id,
            today,
            command.DueDate,
            $"Factura de albarán {note.Number}",
            command.CreatedBy);

        foreach (var noteLine in note.Lines)
        {
            // Busca precio en la línea del pedido origen
            var orderLine = order?.Lines.FirstOrDefault(l => l.Id == noteLine.SalesOrderLineId);
            decimal unitPrice;
            decimal taxRate;

            if (orderLine is not null)
            {
                unitPrice = orderLine.UnitPrice;
                taxRate = orderLine.TaxRate;
            }
            else
            {
                // Fallback: precio actual del catálogo
                var item = await _items.GetByIdAsync(noteLine.ItemId, cancellationToken);
                unitPrice = item?.SalePrice ?? 0m;
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
        return GetSalesInvoiceByIdHandler.ToDto(saved!);
    }
}
