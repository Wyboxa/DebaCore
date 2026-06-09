namespace Debales.Application.Catalog.Queries.GetItemPrice;

public sealed record GetItemPriceQuery(Guid ItemId, Guid? PriceListId);
