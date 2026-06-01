using Debales.Domain.Catalog;
using Debales.Domain.Common;

namespace Debales.Domain.Inventory;

public sealed class StockMovement : AuditableEntity
{
    private StockMovement() { }

    public string Number { get; private set; } = null!;
    public StockMovementType Type { get; private set; }

    // Snapshot of item at movement time
    public Guid ItemId { get; private set; }
    public string ItemCode { get; private set; } = null!;
    public string ItemName { get; private set; } = null!;

    public Guid WarehouseId { get; private set; }
    public Guid? LocationId { get; private set; }

    public DateOnly Date { get; private set; }

    // Signed quantity: positive = stock in, negative = stock out
    public decimal Quantity { get; private set; }

    public string? Reference { get; private set; }
    public string? Notes { get; private set; }

    // Navigation (EF only)
    public Item? Item { get; private set; }
    public Warehouse? Warehouse { get; private set; }
    public WarehouseLocation? Location { get; private set; }

    public static StockMovement Create(
        string number, StockMovementType type,
        Guid itemId, string itemCode, string itemName,
        Guid warehouseId, Guid? locationId,
        DateOnly date, decimal quantity,
        string? reference, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (itemId == Guid.Empty) throw new ArgumentException("Item is required.", nameof(itemId));
        if (warehouseId == Guid.Empty) throw new ArgumentException("Warehouse is required.", nameof(warehouseId));
        if (quantity == 0) throw new ArgumentException("Quantity cannot be zero.", nameof(quantity));

        return new StockMovement
        {
            Number = number.Trim().ToUpper(),
            Type = type,
            ItemId = itemId,
            ItemCode = itemCode,
            ItemName = itemName,
            WarehouseId = warehouseId,
            LocationId = locationId,
            Date = date,
            Quantity = quantity,
            Reference = reference?.Trim(),
            Notes = notes?.Trim(),
            CreatedBy = createdBy
        };
    }
}
