using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesCreditNoteById;

public sealed class GetSalesCreditNoteByIdHandler
{
    private readonly ISalesCreditNoteRepository _notes;

    public GetSalesCreditNoteByIdHandler(ISalesCreditNoteRepository notes) => _notes = notes;

    public async Task<SalesCreditNoteDetailDto?> Handle(GetSalesCreditNoteByIdQuery query, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(query.Id, cancellationToken);
        return note is null ? null : ToDto(note);
    }

    internal static SalesCreditNoteDetailDto ToDto(SalesCreditNote note) => new(
        note.Id,
        note.Number,
        note.CustomerId,
        note.Customer?.Name ?? string.Empty,
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

    internal static SalesCreditNoteSummaryDto ToSummaryDto(SalesCreditNote note) => new(
        note.Id,
        note.Number,
        note.CustomerId,
        note.Customer?.Name ?? string.Empty,
        note.OriginalInvoiceId,
        note.OriginalInvoice?.Number ?? string.Empty,
        note.Date,
        note.Status,
        StatusLabel(note.Status),
        note.Total);

    private static SalesCreditNoteLineSummaryDto ToLineDto(SalesCreditNoteLine l) => new(
        l.Id, l.SortOrder,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity, l.UnitPrice, l.TaxRate,
        l.LineSubtotal, l.LineTaxAmount, l.LineTotal);

    internal static string StatusLabel(SalesCreditNoteStatus s) => s switch
    {
        SalesCreditNoteStatus.Draft => "Borrador",
        SalesCreditNoteStatus.Posted => "Contabilizada",
        SalesCreditNoteStatus.Cancelled => "Cancelada",
        _ => s.ToString()
    };
}
