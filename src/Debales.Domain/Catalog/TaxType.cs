using Debales.Domain.Common;

namespace Debales.Domain.Catalog;

public sealed class TaxType : Entity
{
    private TaxType() { }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public decimal Rate { get; private set; }
    public bool IsActive { get; private set; } = true;

    public static TaxType Create(string code, string name, decimal rate, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (rate < 0 || rate > 100) throw new ArgumentException("Tax rate must be between 0 and 100.", nameof(rate));
        return new TaxType { Code = code.Trim().ToUpper(), Name = name.Trim(), Rate = rate, CreatedBy = createdBy };
    }

    public void Update(string code, string name, decimal rate, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (rate < 0 || rate > 100) throw new ArgumentException("Tax rate must be between 0 and 100.", nameof(rate));
        Code = code.Trim().ToUpper();
        Name = name.Trim();
        Rate = rate;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy) { IsActive = false; SetUpdated(updatedBy); }
    public void Activate(string updatedBy) { IsActive = true; SetUpdated(updatedBy); }
}
