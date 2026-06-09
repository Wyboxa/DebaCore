using Debales.Domain.Common;

namespace Debales.Domain.Sales;

public sealed class PaymentMethod : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Code { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    private PaymentMethod() { }

    public static PaymentMethod Create(string name, string? code, string? description, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        return new PaymentMethod
        {
            Name = name.Trim(),
            Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim().ToUpperInvariant(),
            Description = description?.Trim(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Update(string name, string? code, string? description, bool isActive, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        Name = name.Trim();
        Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim().ToUpperInvariant();
        Description = description?.Trim();
        IsActive = isActive;
        SetUpdated(updatedBy);
    }

    public void Delete(string deletedBy) => SoftDelete(deletedBy);
}
