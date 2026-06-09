using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class PriceList : AuditableEntity
{
    private PriceList() { }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateOnly? ValidFrom { get; private set; }
    public DateOnly? ValidTo { get; private set; }

    // EF navigation — do not mutate directly outside of domain methods
    public ICollection<ItemPrice> Items { get; private set; } = new List<ItemPrice>();

    public static PriceList Create(
        string name, string? description,
        DateOnly? validFrom, DateOnly? validTo, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (validFrom.HasValue && validTo.HasValue && validTo < validFrom)
            throw new ArgumentException("La fecha fin no puede ser anterior a la fecha de inicio.");

        return new PriceList
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            ValidFrom = validFrom,
            ValidTo = validTo,
            CreatedBy = createdBy
        };
    }

    public void Update(
        string name, string? description,
        DateOnly? validFrom, DateOnly? validTo, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (validFrom.HasValue && validTo.HasValue && validTo < validFrom)
            throw new ArgumentException("La fecha fin no puede ser anterior a la fecha de inicio.");

        Name = name.Trim();
        Description = description?.Trim();
        ValidFrom = validFrom;
        ValidTo = validTo;
        SetUpdated(updatedBy);
    }

    public void Activate(string updatedBy) { IsActive = true; SetUpdated(updatedBy); }
    public void Deactivate(string updatedBy) { IsActive = false; SetUpdated(updatedBy); }
    public void Delete(string deletedBy) => SoftDelete(deletedBy);

    public void SetItemPrice(Guid itemId, decimal price, string updatedBy)
    {
        if (price < 0) throw new ArgumentException("El precio no puede ser negativo.", nameof(price));

        var existing = Items.FirstOrDefault(ip => ip.ItemId == itemId);
        if (existing is not null)
        {
            existing.UpdatePrice(price, updatedBy);
            return;
        }

        Items.Add(ItemPrice.Create(Id, itemId, price, updatedBy));
    }

    public void RemoveItemPrice(Guid itemId)
    {
        var ip = Items.FirstOrDefault(ip => ip.ItemId == itemId)
            ?? throw new KeyNotFoundException("Artículo no encontrado en esta tarifa.");
        Items.Remove(ip);
    }
}
