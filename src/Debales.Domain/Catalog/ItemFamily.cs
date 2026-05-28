using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class ItemFamily : Entity
{
    private ItemFamily() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;

    public static ItemFamily Create(string code, string name, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new ItemFamily
        {
            Code = code.Trim().ToUpper(),
            Name = name.Trim(),
            CreatedBy = createdBy
        };
    }

    public void Update(string code, string name, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Code = code.Trim().ToUpper();
        Name = name.Trim();
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy) { IsActive = false; SetUpdated(updatedBy); }
    public void Activate(string updatedBy) { IsActive = true; SetUpdated(updatedBy); }
}
