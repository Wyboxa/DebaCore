 using Debales.Domain.Common;

namespace Debales.Domain.Inventory;

public sealed class WarehouseLocation : AuditableEntity
{
    private WarehouseLocation() { }

    public Guid WarehouseId { get; private set; }
    public string Code { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Warehouse? Warehouse { get; private set; }

    public static WarehouseLocation Create(Guid warehouseId, string code, string? description, string createdBy)
    {
        if (warehouseId == Guid.Empty) throw new ArgumentException("Warehouse is required.", nameof(warehouseId));
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return new WarehouseLocation
        {
            WarehouseId = warehouseId,
            Code = code.Trim().ToUpper(),
            Description = description?.Trim(),
            CreatedBy = createdBy
        };
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
