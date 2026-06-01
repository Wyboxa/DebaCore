using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesCreditNoteById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.PostSalesCreditNote;

public sealed class PostSalesCreditNoteHandler
{
    private readonly ISalesCreditNoteRepository _notes;
    private readonly IReceivableRepository _receivables;
    private readonly IUnitOfWork _uow;

    public PostSalesCreditNoteHandler(ISalesCreditNoteRepository notes, IReceivableRepository receivables, IUnitOfWork uow)
    {
        _notes = notes;
        _receivables = receivables;
        _uow = uow;
    }

    public async Task<SalesCreditNoteDetailDto> Handle(PostSalesCreditNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Rectificativa '{command.Id}' no encontrada.");

        note.Post(command.UpdatedBy);

        // Creates a negative receivable to offset the original invoice's receivable
        var receivableNumber = await _receivables.GetNextNumberAsync(cancellationToken);
        var receivable = Receivable.Create(
            receivableNumber,
            note.OriginalInvoiceId,
            note.CustomerId,
            note.Date,
            -note.Total,
            command.UpdatedBy);

        await _receivables.AddAsync(receivable, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetSalesCreditNoteByIdHandler.ToDto(saved!);
    }
}
