using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesCreditNoteById;

namespace Debales.Application.Sales.Queries.GetSalesCreditNotes;

public sealed class GetSalesCreditNotesHandler
{
    private readonly ISalesCreditNoteRepository _notes;

    public GetSalesCreditNotesHandler(ISalesCreditNoteRepository notes) => _notes = notes;

    public async Task<PagedResult<SalesCreditNoteSummaryDto>> Handle(GetSalesCreditNotesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _notes.SearchAsync(query.Search, query.CustomerId, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(GetSalesCreditNoteByIdHandler.ToSummaryDto).ToList();
        return new PagedResult<SalesCreditNoteSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
