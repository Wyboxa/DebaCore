using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class ItemPrice : Entity
{
    private ItemPrice() { }

    public Guid PriceListId { get; private set; }
    public Guid ItemId { get; private set; }
    public decimal Price { get; private set; }

    // Navigation (EF only)
    public PriceList PriceList { get; private set; } = null!;
    public Item Item { get; private set; } = null!;

    internal static ItemPrice Create(Guid priceListId, Guid itemId, decimal price, string createdBy)
        => new() { PriceListId = priceListId, ItemId = itemId, Price = price, CreatedBy = createdBy };

    internal void UpdatePrice(decimal price, string updatedBy)
    {
        Price = price;
        SetUpdated(updatedBy);
    }
}
