using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesQuoteById;

namespace Debales.Application.Sales.Queries.GetSalesQuotes;

public sealed class GetSalesQuotesHandler
{
    private readonly ISalesQuoteRepository _quotes;

    public GetSalesQuotesHandler(ISalesQuoteRepository quotes) => _quotes = quotes;

    public async Task<PagedResult<SalesQuoteSummaryDto>> Handle(GetSalesQuotesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _quotes.SearchAsync(
            query.Search, query.CustomerId, query.Status,
            query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items.Select(q => new SalesQuoteSummaryDto(
            q.Id, q.Number,
            q.CustomerId, q.Customer?.Name ?? string.Empty,
            q.Date, q.ValidUntil,
            q.Status, GetSalesQuoteByIdHandler.StatusLabel(q.Status),
            q.Total, q.ConvertedToOrderId)).ToList();

        return new PagedResult<SalesQuoteSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
