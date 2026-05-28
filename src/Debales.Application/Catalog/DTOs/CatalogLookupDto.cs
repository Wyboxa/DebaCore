namespace Debales.Application.Catalog.DTOs;

public sealed record CatalogLookupDto(Guid Id, string Code, string Name);

public sealed record TaxTypeLookupDto(Guid Id, string Code, string Name, decimal Rate);
