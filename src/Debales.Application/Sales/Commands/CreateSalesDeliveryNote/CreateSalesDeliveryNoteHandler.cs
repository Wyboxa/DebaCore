using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesDeliveryNote;

public sealed class CreateSalesDeliveryNoteHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly IItemRepository _items;
    private readonly INumberSeriesRepository _series;
    private readonly IUnitOfWork _uow;

    public CreateSalesDeliveryNoteHandler(ISalesDeliveryNoteRepository notes, IItemRepository items, INumberSeriesRepository series, IUnitOfWork uow)
    {
        _notes = notes;
        _items = items;
        _series = series;
        _uow = uow;
    }

    public async Task<SalesDeliveryNoteDetailDto> Handle(CreateSalesDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("Un albarán debe tener al menos una línea.");

        var serie = await _series.GetByCodeAsync("ALV", cancellationToken)
            ?? throw new InvalidOperationException("Serie 'ALV' no encontrada. Configure las series documentales en Configuración.");
        var number = serie.Consume(command.CreatedBy);
        _series.Update(serie);

        var note = SalesDeliveryNote.Create(
            number,
            command.CustomerId,
            command.SalesOrderId,
            command.Date,
            command.Notes,
            command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            note.AddLine(
                lineReq.SalesOrderLineId,
                lineReq.SalesOrderId,
                item.Id, item.Code, item.Name,
                lineReq.Description,
                lineReq.Quantity);
        }

        await _notes.AddAsync(note, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesDeliveryNoteByIdHandler.ToDto(saved!);
    }
}
