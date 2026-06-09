using Debales.Application.Common;
using Debales.Application.Sales.Commands.CreatePaymentMethod;
using Debales.Application.Sales.DTOs;

namespace Debales.Application.Sales.Queries.GetPaymentMethods;

public sealed class GetPaymentMethodsHandler(IPaymentMethodRepository repository)
{
    public async Task<PagedResult<PaymentMethodSummaryDto>> Handle(GetPaymentMethodsQuery query, CancellationToken cancellationToken = default)
    {
        var paged = await repository.SearchAsync(query.Search, query.IsActive, query.Page, query.PageSize, cancellationToken);
        var items = paged.Items.Select(pm => new PaymentMethodSummaryDto(pm.Id, pm.Name, pm.Code, pm.Description, pm.IsActive)).ToList();
        return new PagedResult<PaymentMethodSummaryDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
