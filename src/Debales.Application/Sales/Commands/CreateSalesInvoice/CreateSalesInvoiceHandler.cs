using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesInvoice;

public sealed class CreateSalesInvoiceHandler
{
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public CreateSalesInvoiceHandler(ISalesInvoiceRepository invoices, IItemRepository items, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _invoices = invoices;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<SalesInvoiceDetailDto> Handle(CreateSalesInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("An invoice must have at least one line.");

        var serie = await _series.GetByCodeAsync("FV", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'FV' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var invoice = SalesInvoice.Create(
            number, command.CustomerId, command.SalesDeliveryNoteId,
            command.Date, command.DueDate, command.Notes, command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            invoice.AddLine(
                item.Id, item.Code, item.Name,
                lineReq.Description,
                lineReq.Quantity, lineReq.UnitPrice, lineReq.TaxRate);
        }

        await _invoices.AddAsync(invoice, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetSalesInvoiceByIdHandler.ToDto(saved!);
    }
}
