using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateSalesDeliveryNote;

public sealed class CreateSalesDeliveryNoteHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public CreateSalesDeliveryNoteHandler(ISalesDeliveryNoteRepository notes, IItemRepository items, IUnitOfWork uow)
    {
        _notes = notes;
        _items = items;
        _uow = uow;
    }

    public async Task<SalesDeliveryNoteDetailDto> Handle(CreateSalesDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("Un albarán debe tener al menos una línea.");

        var number = await _notes.GetNextNumberAsync(cancellationToken);

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
