using Debales.Application.Catalog.DTOs;

namespace Debales.Application.Catalog.Queries.GetCatalogLookups;

public sealed class GetCatalogLookupsHandler
{
    private readonly IItemFamilyRepository _families;
    private readonly IUnitOfMeasureRepository _uoms;
    private readonly ITaxTypeRepository _taxTypes;

    public GetCatalogLookupsHandler(
        IItemFamilyRepository families,
        IUnitOfMeasureRepository uoms,
        ITaxTypeRepository taxTypes)
    {
        _families = families;
        _uoms = uoms;
        _taxTypes = taxTypes;
    }

    public async Task<CatalogLookupsDto> Handle(CancellationToken cancellationToken = default)
    {
        var families = await _families.GetAllActiveAsync(cancellationToken);
        var uoms = await _uoms.GetAllActiveAsync(cancellationToken);
        var taxTypes = await _taxTypes.GetAllActiveAsync(cancellationToken);

        return new CatalogLookupsDto(
            families.Select(f => new CatalogLookupDto(f.Id, f.Code, f.Name)).ToList(),
            uoms.Select(u => new CatalogLookupDto(u.Id, u.Code, u.Name)).ToList(),
            taxTypes.Select(t => new TaxTypeLookupDto(t.Id, t.Code, t.Name, t.Rate)).ToList());
    }
}

public sealed record CatalogLookupsDto(
    IReadOnlyList<CatalogLookupDto> Families,
    IReadOnlyList<CatalogLookupDto> UnitsOfMeasure,
    IReadOnlyList<TaxTypeLookupDto> TaxTypes);
