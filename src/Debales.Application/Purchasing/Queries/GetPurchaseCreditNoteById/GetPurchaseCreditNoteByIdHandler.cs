using Debales.Application.Purchasing.DTOs;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseCreditNoteById;

public sealed class GetPurchaseCreditNoteByIdHandler
{
    private readonly IPurchaseCreditNoteRepository _notes;

    public GetPurchaseCreditNoteByIdHandler(IPurchaseCreditNoteRepository notes) => _notes = notes;

    public async Task<PurchaseCreditNoteDetailDto?> Handle(GetPurchaseCreditNoteByIdQuery query, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(query.Id, cancellationToken);
        return note is null ? null : ToDto(note);
    }

    internal static PurchaseCreditNoteDetailDto ToDto(PurchaseCreditNote note) => new(
        note.Id,
        note.Number,
        note.SupplierId,
        note.Supplier?.Name ?? string.Empty,
        note.OriginalInvoiceId,
        note.OriginalInvoice?.Number ?? string.Empty,
        note.Date,
        note.Status,
        StatusLabel(note.Status),
        note.Reason,
        note.Lines.Select(ToLineDto).ToList(),
        note.Subtotal,
        note.TaxAmount,
        note.Total,
        note.CreatedAt,
        note.CreatedBy,
        note.UpdatedAt,
        note.UpdatedBy);

    internal static PurchaseCreditNoteSummaryDto ToSummaryDto(PurchaseCreditNote note) => new(
        note.Id,
        note.Number,
        note.SupplierId,
        note.Supplier?.Name ?? string.Empty,
        note.OriginalInvoiceId,
        note.OriginalInvoice?.Number ?? string.Empty,
        note.Date,
        note.Status,
        StatusLabel(note.Status),
        note.Total);

    private static PurchaseCreditNoteLineSummaryDto ToLineDto(PurchaseCreditNoteLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(PurchaseCreditNoteStatus s) => s switch
    {
        PurchaseCreditNoteStatus.Draft => "Borrador",
        PurchaseCreditNoteStatus.Posted => "Contabilizada",
        PurchaseCreditNoteStatus.Cancelled => "Cancelada",
        _ => s.ToString()
    };
}
