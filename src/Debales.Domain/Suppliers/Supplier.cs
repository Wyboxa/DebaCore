using Debales.Domain.Common;

namespace Debales.Domain.Suppliers;

public sealed class Supplier : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? TaxId { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public string? Website { get; private set; }
    public string? ContactName { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }
    public SupplierAddress? Address { get; private set; }

    private Supplier() { }

    public static Supplier Create(
        string name,
        string? taxId,
        string? phone,
        string? email,
        string? contactName,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del proveedor no puede estar vacío.", nameof(name));

        return new Supplier
        {
            Name = name.Trim(),
            TaxId = taxId?.Trim(),
            Phone = phone?.Trim(),
            Email = email?.Trim().ToLowerInvariant(),
            ContactName = contactName?.Trim(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Update(
        string name,
        string? taxId,
        string? phone,
        string? email,
        string? website,
        string? contactName,
        string? notes,
        string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del proveedor no puede estar vacío.", nameof(name));

        Name = name.Trim();
        TaxId = taxId?.Trim();
        Phone = phone?.Trim();
        Email = email?.Trim().ToLowerInvariant();
        Website = website?.Trim();
        ContactName = contactName?.Trim();
        Notes = notes?.Trim();
        SetUpdated(updatedBy);
    }

    public void SetAddress(SupplierAddress? address, string updatedBy)
    {
        Address = address;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
