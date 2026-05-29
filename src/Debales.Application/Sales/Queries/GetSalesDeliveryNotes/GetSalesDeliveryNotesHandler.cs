using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesDeliveryNoteById;

namespace Debales.Application.Sales.Queries.GetSalesDeliveryNotes;

public sealed class GetSalesDeliveryNotesHandler
{
    private readonly ISalesDeliveryNoteRepository _notes;

    public GetSalesDeliveryNotesHandler(ISalesDeliveryNoteRepository notes) => _notes = notes;

    public async Task<PagedResult<SalesDeliveryNoteSummaryDto>> Handle(GetSalesDeliveryNotesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _notes.SearchAsync(
            query.Search, query.CustomerId, query.Status,
            query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items.Select(n => new SalesDeliveryNoteSummaryDto(
            n.Id, n.Number,
            n.CustomerId, n.Customer?.Name ?? string.Empty,
            n.SalesOrderId, n.SalesOrder?.Number,
            n.Date,
            n.Status, GetSalesDeliveryNoteByIdHandler.StatusLabel(n.Status))).ToList();

        return new PagedResult<SalesDeliveryNoteSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
