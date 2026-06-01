using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseCreditNoteById;

namespace Debales.Application.Purchasing.Queries.GetPurchaseCreditNotes;

public sealed class GetPurchaseCreditNotesHandler
{
    private readonly IPurchaseCreditNoteRepository _notes;

    public GetPurchaseCreditNotesHandler(IPurchaseCreditNoteRepository notes) => _notes = notes;

    public async Task<PagedResult<PurchaseCreditNoteSummaryDto>> Handle(GetPurchaseCreditNotesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _notes.SearchAsync(query.Search, query.SupplierId, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(GetPurchaseCreditNoteByIdHandler.ToSummaryDto).ToList();
        return new PagedResult<PurchaseCreditNoteSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
