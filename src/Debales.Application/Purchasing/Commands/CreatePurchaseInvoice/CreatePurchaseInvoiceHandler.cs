using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.CreatePurchaseInvoice;

public sealed class CreatePurchaseInvoiceHandler
{
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public CreatePurchaseInvoiceHandler(IPurchaseInvoiceRepository invoices, IItemRepository items, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _invoices = invoices;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<PurchaseInvoiceDetailDto> Handle(CreatePurchaseInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("An invoice must have at least one line.");

        var serie = await _series.GetByCodeAsync("FC", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'FC' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var invoice = PurchaseInvoice.Create(
            number, command.SupplierInvoiceNumber, command.SupplierId,
            command.PurchaseDeliveryNoteId, command.Date, command.DueDate,
            command.Notes, command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            invoice.AddLine(item.Id, item.Code, item.Name, lineReq.Description,
                lineReq.Quantity, lineReq.UnitPrice, lineReq.TaxRate);
        }

        await _invoices.AddAsync(invoice, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetPurchaseInvoiceByIdHandler.ToDto(saved!);
    }
}
