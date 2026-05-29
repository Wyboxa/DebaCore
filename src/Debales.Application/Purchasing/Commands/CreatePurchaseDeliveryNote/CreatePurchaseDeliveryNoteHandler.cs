using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.CreatePurchaseDeliveryNote;

public sealed class CreatePurchaseDeliveryNoteHandler
{
    private readonly IPurchaseDeliveryNoteRepository _notes;
    private readonly IItemRepository _items;
    private readonly IUnitOfWork _uow;

    public CreatePurchaseDeliveryNoteHandler(IPurchaseDeliveryNoteRepository notes, IItemRepository items, IUnitOfWork uow)
    {
        _notes = notes;
        _items = items;
        _uow = uow;
    }

    public async Task<PurchaseDeliveryNoteDetailDto> Handle(CreatePurchaseDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new ArgumentException("Un albarán debe tener al menos una línea.");

        var number = await _notes.GetNextNumberAsync(cancellationToken);

        var note = PurchaseDeliveryNote.Create(
            number,
            command.SupplierId,
            command.PurchaseOrderId,
            command.Date,
            command.Notes,
            command.CreatedBy);

        foreach (var lineReq in command.Lines)
        {
            var item = await _items.GetByIdAsync(lineReq.ItemId, cancellationToken)
                ?? throw new InvalidOperationException($"Artículo '{lineReq.ItemId}' no encontrado.");

            note.AddLine(
                lineReq.PurchaseOrderLineId,
                lineReq.PurchaseOrderId,
                item.Id, item.Code, item.Name,
                lineReq.Description,
                lineReq.Quantity);
        }

        await _notes.AddAsync(note, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetPurchaseDeliveryNoteByIdHandler.ToDto(saved!);
    }
}
