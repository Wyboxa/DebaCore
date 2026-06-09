using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesCreditNoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesCreditNote;

public sealed class CreateSalesCreditNoteHandler
{
    private readonly ISalesCreditNoteRepository _notes;
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public CreateSalesCreditNoteHandler(
        ISalesCreditNoteRepository notes, ISalesInvoiceRepository invoices,
        IItemRepository items, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _notes = notes;
        _invoices = invoices;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<SalesCreditNoteDetailDto> Handle(CreateSalesCreditNoteCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("A credit note must have at least one line.");

        var invoice = await _invoices.GetByIdAsync(command.OriginalInvoiceId, cancellationToken)
            ?? throw new InvalidOperationException($"Factura original '{command.OriginalInvoiceId}' no encontrada.");

        if (invoice.Status != SalesInvoiceStatus.Posted)
            throw new InvalidOperationException("Credit notes can only be created for posted invoices.");

        var serie = await _series.GetByCodeAsync("RV", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'RV' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var note = SalesCreditNote.Create(
            number, command.CustomerId, command.OriginalInvoiceId,
            command.Date, command.Reason, command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            note.AddLine(item.Id, item.Code, item.Name, lineReq.Description,
                lineReq.Quantity, lineReq.UnitPrice, lineReq.TaxRate);
        }

        await _notes.AddAsync(note, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesCreditNoteByIdHandler.ToDto(saved!);
    }
}
