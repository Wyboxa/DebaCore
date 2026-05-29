using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;

namespace Debales.Application.Sales.Commands.PostSalesDeliveryNote;

public sealed class PostSalesDeliveryNoteHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;
    private readonly IUnitOfWork _uow;

    public PostSalesDeliveryNoteHandler(ISalesDeliveryNoteRepository notes, IUnitOfWork uow)
    {
        _notes = notes;
        _uow = uow;
    }

    public async Task<SalesDeliveryNoteDetailDto> Handle(PostSalesDeliveryNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException("Albarán de venta no encontrado.");

        note.Post(command.UpdatedBy);
        _notes.Update(note);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesDeliveryNoteByIdHandler.ToDto(saved!);
    }
}
