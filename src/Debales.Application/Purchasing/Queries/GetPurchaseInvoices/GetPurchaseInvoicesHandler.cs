using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;

namespace Debales.Application.Purchasing.Queries.GetPurchaseInvoices;

public sealed class GetPurchaseInvoicesHandler
{
    private readonly IPurchaseInvoiceRepository _invoices;

    public GetPurchaseInvoicesHandler(IPurchaseInvoiceRepository invoices) => _invoices = invoices;

    public async Task<PagedResult<PurchaseInvoiceSummaryDto>> Handle(GetPurchaseInvoicesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _invoices.SearchAsync(query.Search, query.SupplierId, query.Status, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(GetPurchaseInvoiceByIdHandler.ToSummaryDto).ToList();
        return new PagedResult<PurchaseInvoiceSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
