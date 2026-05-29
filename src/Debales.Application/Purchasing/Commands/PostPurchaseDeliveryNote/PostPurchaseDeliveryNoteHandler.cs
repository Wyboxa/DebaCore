using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;

namespace Debales.Application.Purchasing.Commands.PostPurchaseDeliveryNote;

public sealed class PostPurchaseDeliveryNoteHandler
{
    private readonly IPurchaseDeliveryNoteRepository _notes;
    private readonly IUnitOfWork _uow;

    public PostPurchaseDeliveryNoteHandler(IPurchaseDeliveryNoteRepository notes, IUnitOfWork uow)
    {
        _notes = notes;
        _uow = uow;
    }

    public async Task<PurchaseDeliveryNoteDetailDto> Handle(PostPurchaseDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Albarán de compra no encontrado.");

        note.Post(command.UpdatedBy);
        _notes.Update(note);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetPurchaseDeliveryNoteByIdHandler.ToDto(saved!);
    }
}
