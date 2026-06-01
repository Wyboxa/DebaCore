using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Queries.GetReceivables;

public sealed class GetReceivablesHandler
{
    private readonly IReceivableRepository _receivables;

    public GetReceivablesHandler(IReceivableRepository receivables) => _receivables = receivables;

    public async Task<PagedResult<ReceivableSummaryDto>> Handle(GetReceivablesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _receivables.SearchAsync(query.Search, query.CustomerId, query.Status, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(ToSummaryDto).ToList();
        return new PagedResult<ReceivableSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }

    internal static ReceivableSummaryDto ToSummaryDto(Receivable r) => new(
        r.Id,
        r.Number,
        r.SalesInvoiceId,
        r.SalesInvoice?.Number ?? string.Empty,
        r.CustomerId,
        r.Customer?.Name ?? string.Empty,
        r.DueDate,
        r.OriginalAmount,
        r.PaidAmount,
        r.PendingAmount,
        r.Status,
        StatusLabel(r.Status));

    internal static string StatusLabel(ReceivableStatus s) => s switch
    {
        ReceivableStatus.Pending => "Pendiente",
        ReceivableStatus.Partial => "Parcial",
        ReceivableStatus.Settled => "Cobrado",
        ReceivableStatus.Defaulted => "Impagado",
        ReceivableStatus.Cancelled => "Cancelado",
        _ => s.ToString()
    };
}
