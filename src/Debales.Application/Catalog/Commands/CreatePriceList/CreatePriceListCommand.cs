namespace Debales.Application.Catalog.Commands.CreatePriceList;

public sealed record CreatePriceListCommand(
    string Name,
    string? Description,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    string CreatedBy);
