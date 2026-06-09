using Debales.Application.Catalog.DTOs;

namespace Debales.Application.Catalog.Queries.GetCustomerItemCodes;

public sealed class GetCustomerItemCodesHandler
{
    private readonly ICustomerItemCodeRepository _codes;

    public GetCustomerItemCodesHandler(ICustomerItemCodeRepository codes) => _codes = codes;

    public async Task<List<CustomerItemCodeDto>> Handle(
        GetCustomerItemCodesQuery query, CancellationToken cancellationToken = default)
    {
        var list = await _codes.GetByCustomerIdAsync(query.CustomerId, cancellationToken);
        return list
            .OrderBy(c => c.Item?.Code)
            .Select(c => new CustomerItemCodeDto(
                c.Id, c.CustomerId, c.ItemId,
                c.Item?.Code ?? "", c.Item?.Name ?? "",
                c.CustomerCode, c.Description,
                c.CreatedAt))
            .ToList();
    }
}
