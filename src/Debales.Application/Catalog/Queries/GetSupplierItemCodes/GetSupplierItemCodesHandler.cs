using Debales.Application.Catalog.DTOs;

namespace Debales.Application.Catalog.Queries.GetSupplierItemCodes;

public sealed class GetSupplierItemCodesHandler
{
    private readonly ISupplierItemCodeRepository _codes;

    public GetSupplierItemCodesHandler(ISupplierItemCodeRepository codes) => _codes = codes;

    public async Task<List<SupplierItemCodeDto>> Handle(
        GetSupplierItemCodesQuery query, CancellationToken cancellationToken = default)
    {
        var list = await _codes.GetBySupplierIdAsync(query.SupplierId, cancellationToken);
        return list
            .OrderBy(c => c.Item?.Code)
            .Select(c => new SupplierItemCodeDto(
                c.Id, c.SupplierId, c.ItemId,
                c.Item?.Code ?? "", c.Item?.Name ?? "",
                c.SupplierCode, c.Description,
                c.CreatedAt))
            .ToList();
    }
}
