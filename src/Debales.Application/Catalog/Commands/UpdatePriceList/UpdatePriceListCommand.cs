namespace Debales.Application.Catalog.Commands.UpdatePriceList;

public sealed record UpdatePriceListCommand(
    Guid Id,
    string Name,
    string? Description,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    bool IsActive,
    string UpdatedBy);
