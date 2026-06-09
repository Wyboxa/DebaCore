using Debales.Application.Common;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Queries.GetPaymentTerms;

public sealed class GetPaymentTermsHandler
{
    private readonly IPaymentTermRepository _paymentTerms;

    public GetPaymentTermsHandler(IPaymentTermRepository paymentTerms) => _paymentTerms = paymentTerms;

    public async Task<PagedResult<PaymentTermSummaryDto>> Handle(GetPaymentTermsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _paymentTerms.SearchAsync(query.Search, query.IsActive, query.Page, query.PageSize, cancellationToken);
        var dtos = result.Items
            .Select(pt => new PaymentTermSummaryDto(pt.Id, pt.Name, pt.Description, pt.IsActive, pt.Lines.Count))
            .ToList();
        return new PagedResult<PaymentTermSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
