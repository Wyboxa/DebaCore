using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;

namespace Debales.Application.Purchasing.Queries.GetSupplierPayments;

public sealed class GetSupplierPaymentsHandler
{
    private readonly ISupplierPaymentRepository _payments;

    public GetSupplierPaymentsHandler(ISupplierPaymentRepository payments) => _payments = payments;

    public async Task<PagedResult<SupplierPaymentSummaryDto>> Handle(GetSupplierPaymentsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _payments.SearchAsync(query.Search, query.SupplierId, query.Page, query.PageSize, cancellationToken);
        var items = result.Items.Select(p => new SupplierPaymentSummaryDto(
            p.Id, p.Number,
            p.SupplierId, p.Supplier?.Name ?? string.Empty,
            p.PayableId, p.Payable?.Number,
            p.Date, p.Amount, p.Reference)).ToList();
        return new PagedResult<SupplierPaymentSummaryDto>(items, result.TotalCount, result.Page, result.PageSize);
    }
}
