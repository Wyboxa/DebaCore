using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;

public sealed class GetSalesDeliveryNoteByIdHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;

    public GetSalesDeliveryNoteByIdHandler(ISalesDeliveryNoteRepository notes) => _notes = notes;

    public async Task<SalesDeliveryNoteDetailDto?> Handle(GetSalesDeliveryNoteByIdQuery query, CancellationToken cancellationToken = default)
    {
        var note = await _notes.GetByIdAsync(query.Id, cancellationToken);
        return note is null ? null : ToDto(note);
    }

    internal static SalesDeliveryNoteDetailDto ToDto(SalesDeliveryNote note) => new(
        note.Id,
        note.Number,
        note.CustomerId,
        note.Customer?.Name ?? string.Empty,
        note.SalesOrderId,
        note.SalesOrder?.Number,
        note.Date,
        note.Status,
        StatusLabel(note.Status),
        note.Notes,
        note.Lines.Select(ToLineDto).ToList(),
        note.CreatedAt,
        note.CreatedBy,
        note.UpdatedAt,
        note.UpdatedBy);

    private static SalesDeliveryNoteLineSummaryDto ToLineDto(SalesDeliveryNoteLine l) => new(
        l.Id, l.SortOrder,
        l.SalesOrderLineId, l.SalesOrderId,
        l.ItemId, l.ItemCode, l.ItemName, l.Description,
        l.Quantity);

    internal static string StatusLabel(SalesDeliveryNoteStatus s) => s switch
    {
        SalesDeliveryNoteStatus.Draft => "Borrador",
        SalesDeliveryNoteStatus.Posted => "Emitido",
        SalesDeliveryNoteStatus.Cancelled => "Cancelado",
        _ => s.ToString()
    };
}
