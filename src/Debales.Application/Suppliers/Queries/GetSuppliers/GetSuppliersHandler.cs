using Debales.Application.Common;
using Debales.Application.Suppliers.DTOs;

namespace Debales.Application.Suppliers.Queries.GetSuppliers;

public sealed class GetSuppliersHandler
{
    private readonly ISupplierRepository _suppliers;

    public GetSuppliersHandler(ISupplierRepository suppliers) => _suppliers = suppliers;

    public async Task<PagedResult<SupplierSummaryDto>> Handle(GetSuppliersQuery query, CancellationToken cancellationToken = default)
    {
        var paged = await _suppliers.SearchAsync(query.Search, query.Page, query.PageSize, cancellationToken);

        var items = paged.Items.Select(s => new SupplierSummaryDto(
            s.Id, s.Name, s.TaxId, s.Phone, s.Email, s.ContactName, s.IsActive, s.CreatedAt)).ToList();

        return new PagedResult<SupplierSummaryDto>(items, paged.TotalCount, paged.Page, paged.PageSize);
    }
}
