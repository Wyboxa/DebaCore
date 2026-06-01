using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Queries.GetPayables;

public sealed class GetPayablesHandler
{
    private readonly IPayableRepository _payables;

    public GetPayablesHandler(IPayableRepository payables) => _payables = payables;

    public async Task<PagedResult<PayableSummaryDto>> Handle(GetPayablesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _payables.SearchAsync(query.Search, query.SupplierId, query.Status, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(ToSummaryDto).ToList();
        return new PagedResult<PayableSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }

    internal static PayableSummaryDto ToSummaryDto(Payable p) => new(
        p.Id,
        p.Number,
        p.PurchaseInvoiceId,
        p.PurchaseInvoice?.Number ?? string.Empty,
        p.SupplierId,
        p.Supplier?.Name ?? string.Empty,
        p.DueDate,
        p.OriginalAmount,
        p.PaidAmount,
        p.PendingAmount,
        p.Status,
        StatusLabel(p.Status));

    internal static string StatusLabel(PayableStatus s) => s switch
    {
        PayableStatus.Pending => "Pendiente",
        PayableStatus.Partial => "Parcial",
        PayableStatus.Settled => "Pagado",
        PayableStatus.Cancelled => "Cancelado",
        _ => s.ToString()
    };
}
