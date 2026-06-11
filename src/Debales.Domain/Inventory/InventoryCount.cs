using Debales.Domain.Common;

namespace Debales.Domain.Inventory;

public sealed class InventoryCount : AuditableEntity
{
    private readonly List<InventoryCountLine> _lines = [];
    private InventoryCount() { }

    public string Number { get; private set; } = null!;
    public DateOnly Date { get; private set; }
    public Guid WarehouseId { get; private set; }
    public InventoryCountStatus Status { get; private set; } = InventoryCountStatus.Open;
    public string? Notes { get; private set; }

    public Warehouse? Warehouse { get; private set; }
    public IReadOnlyList<InventoryCountLine> Lines => _lines.AsReadOnly();

    public static InventoryCount Create(string number, DateOnly date, Guid warehouseId, string? notes, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);
        if (warehouseId == Guid.Empty) throw new ArgumentException("Warehouse is required.", nameof(warehouseId));

        return new InventoryCount
        {
            Number = number.Trim().ToUpperInvariant(),
            Date = date,
            WarehouseId = warehouseId,
            Notes = notes?.Trim(),
            Status = InventoryCountStatus.Open,
            CreatedBy = createdBy
        };
    }

    public void Close(string updatedBy)
    {
        if (Status != InventoryCountStatus.Open)
            throw new InvalidOperationException("Solo se puede cerrar un recuento abierto.");

        Status = InventoryCountStatus.Closed;
        SetUpdated(updatedBy);
    }

    public void AddLine(Guid itemId, string itemCode, string itemName, decimal systemQuantity, string createdBy)
    {
        if (Status != InventoryCountStatus.Open)
            throw new InvalidOperationException("No se pueden añadir líneas a un recuento cerrado.");

        if (_lines.Any(l => l.ItemId == itemId))
            throw new InvalidOperationException($"El artículo '{itemCode}' ya está en el recuento.");

        _lines.Add(InventoryCountLine.Create(Id, itemId, itemCode, itemName, systemQuantity, createdBy));
    }

    public void SetCountedQuantity(Guid itemId, decimal countedQuantity, string updatedBy)
    {
        if (Status != InventoryCountStatus.Open)
            throw new InvalidOperationException("No se puede modificar un recuento cerrado.");

        var line = _lines.FirstOrDefault(l => l.ItemId == itemId)
            ?? throw new InvalidOperationException($"Artículo no encontrado en el recuento.");

        line.SetCounted(countedQuantity, updatedBy);
    }
}
