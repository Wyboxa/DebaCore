using Debales.Domain.Common;

namespace Debales.Domain.Inventory;

public sealed class Warehouse : AuditableEntity
{
    private readonly List<WarehouseLocation> _locations = [];

    private Warehouse() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    public IReadOnlyList<WarehouseLocation> Locations => _locations.AsReadOnly();

    public static Warehouse Create(string code, string name, string? description, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Warehouse
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            Description = description?.Trim(),
            CreatedBy = createdBy
        };
    }

    public void Update(string name, string? description, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        Description = description?.Trim();
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        IsActive = true;
        SetUpdated(updatedBy);
    }
}
