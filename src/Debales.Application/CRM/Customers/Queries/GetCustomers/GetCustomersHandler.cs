using Debales.Application.Common;
using Debales.Application.CRM.Customers.DTOs;

namespace Debales.Application.CRM.Customers.Queries.GetCustomers;

public sealed class GetCustomersHandler
{
    private readonly ICustomerRepository _customers;

    public GetCustomersHandler(ICustomerRepository customers) => _customers = customers;

    public async Task<PagedResult<CustomerSummaryDto>> Handle(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _customers.SearchAsync(query.Search, query.Page, query.PageSize, cancellationToken);

        var dtos = result.Items
            .Select(c => new CustomerSummaryDto(c.Id, c.Name, c.Sector, c.TaxId, c.Phone, c.Email, c.IsActive, c.CreatedAt, c.PriceListId, c.PaymentTermId))
            .ToList();

        return new PagedResult<CustomerSummaryDto>(dtos, result.TotalCount, result.Page, result.PageSize);
    }
}
