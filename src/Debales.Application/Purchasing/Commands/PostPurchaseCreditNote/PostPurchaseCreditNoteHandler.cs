using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseCreditNoteById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.PostPurchaseCreditNote;

public sealed class PostPurchaseCreditNoteHandler
{
    private readonly IPurchaseCreditNoteRepository _notes;
    private readonly IPayableRepository _payables;
    private readonly IUnitOfWork _uow;

    public PostPurchaseCreditNoteHandler(IPurchaseCreditNoteRepository notes, IPayableRepository payables, IUnitOfWork uow)
    {
        _notes = notes;
        _payables = payables;
        _uow = uow;
    }

    public async Task<PurchaseCreditNoteDetailDto> Handle(PostPurchaseCreditNoteCommand command, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Rectificativa '{command.Id}' no encontrada.");

        note.Post(command.UpdatedBy);

        var payableNumber = await _payables.GetNextNumberAsync(cancellationToken);
        var payable = Payable.Create(
            payableNumber, note.OriginalInvoiceId, note.SupplierId,
            note.Date, -note.Total, command.UpdatedBy);

        await _payables.AddAsync(payable, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _notes.GetByIdAsync(note.Id, cancellationToken);
        return GetPurchaseCreditNoteByIdHandler.ToDto(saved!);
    }
}
