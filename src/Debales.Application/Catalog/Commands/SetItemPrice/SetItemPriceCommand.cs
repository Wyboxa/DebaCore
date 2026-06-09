namespace Debales.Application.Catalog.Commands.SetItemPrice;

public sealed record SetItemPriceCommand(
    Guid PriceListId,
    Guid ItemId,
    decimal Price,
    string UpdatedBy);
