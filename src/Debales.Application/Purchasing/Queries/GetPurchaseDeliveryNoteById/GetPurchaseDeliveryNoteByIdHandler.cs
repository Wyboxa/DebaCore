using Debales.Application.Purchasing.DTOs;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;

public sealed class GetPurchaseDeliveryNoteByIdHandler
{
    private readonly IPurchaseDeliveryNoteRepository _notes;

    public GetPurchaseDeliveryNoteByIdHandler(IPurchaseDeliveryNoteRepository notes) => _notes = notes;

    public async Task<PurchaseDeliveryNoteDetailDto?> Handle(GetPurchaseDeliveryNoteByIdQuery query, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(query.Id, cancellationToken);
        return note is null ? null : ToDto(note);
    }

    internal static PurchaseDeliveryNoteDetailDto ToDto(PurchaseDeliveryNote note) => new(
        note.Id,
        note.Number,
        note.SupplierId,
        note.Supplier?.Name ?? string.Empty,
        note.PurchaseOrderId,
        note.PurchaseOrder?.Number,
        note.Date,
        note.Status,
        StatusLabel(note.Status),
        note.Notes,
        note.Lines.Select(ToLineDto).ToList(),
        note.CreatedAt,
        note.CreatedBy,
        note.UpdatedAt,
        note.UpdatedBy);

    private static PurchaseDeliveryNoteLineSummaryDto ToLineDto(PurchaseDeliveryNoteLine l) => new(
        l.Id, l.SortOrder,
        l.PurchaseOrderLineId, l.PurchaseOrderId,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity);

    internal static string StatusLabel(PurchaseDeliveryNoteStatus s) => s switch
    {
        PurchaseDeliveryNoteStatus.Draft => "Borrador",
        PurchaseDeliveryNoteStatus.Posted => "Emitido",
        PurchaseDeliveryNoteStatus.Cancelled => "Cancelado",
        _ => s.ToString()
    };
}
