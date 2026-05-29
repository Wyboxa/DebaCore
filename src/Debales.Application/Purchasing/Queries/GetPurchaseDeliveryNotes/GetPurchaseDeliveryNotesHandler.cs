using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNoteById;

namespace Debales.Application.Purchasing.Queries.GetPurchaseDeliveryNotes;

public sealed class GetPurchaseDeliveryNotesHandler
{
    private readonly IPurchaseDeliveryNoteRepository _notes;

    public GetPurchaseDeliveryNotesHandler(IPurchaseDeliveryNoteRepository notes) => _notes = notes;

    public async Task<PagedResult<PurchaseDeliveryNoteSummaryDto>> Handle(GetPurchaseDeliveryNotesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _notes.SearchAsync(
            query.Search, query.SupplierId, query.Status,
            query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items.Select(n => new PurchaseDeliveryNoteSummaryDto(
            n.Id, n.Number,
            n.SupplierId, n.Supplier?.Name ?? string.Empty,
            n.PurchaseOrderId, n.PurchaseOrder?.Number,
            n.Date,
            n.Status, GetPurchaseDeliveryNoteByIdHandler.StatusLabel(n.Status))).ToList();

        return new PagedResult<PurchaseDeliveryNoteSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
