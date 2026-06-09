namespace Debales.Application.Catalog.Commands.RemoveItemPrice;

public sealed record RemoveItemPriceCommand(
    Guid PriceListId,
    Guid ItemId,
    string UpdatedBy);
