using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;

namespace Debales.Application.Sales.Queries.GetSalesInvoices;

public sealed class GetSalesInvoicesHandler
{
    private readonly ISalesInvoiceRepository _invoices;

    public GetSalesInvoicesHandler(ISalesInvoiceRepository invoices) => _invoices = invoices;

    public async Task<PagedResult<SalesInvoiceSummaryDto>> Handle(GetSalesInvoicesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _invoices.SearchAsync(query.Search, query.CustomerId, query.Status, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(GetSalesInvoiceByIdHandler.ToSummaryDto).ToList();
        return new PagedResult<SalesInvoiceSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
