using Debales.Domain.Common;

namespace Debales.Domain.Core.NumberSeries;

public sealed class NumberSeries : Entity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Prefix { get; private set; } = string.Empty;
    public int Padding { get; private set; }
    public int NextNumber { get; private set; }
    public bool IsActive { get; private set; }

    private NumberSeries() { }

    public static NumberSeries Create(string code, string name, string prefix, int padding, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código de serie no puede estar vacío.", nameof(code));
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("El prefijo no puede estar vacío.", nameof(prefix));
        if (padding is < 1 or > 10)
            throw new ArgumentException("El padding debe estar entre 1 y 10.", nameof(padding));

        return new NumberSeries
        {
            Code = code.Trim().ToUpperInvariant(),
            Name = name.Trim(),
            Prefix = prefix.Trim(),
            Padding = padding,
            NextNumber = 1,
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public string GetNextFormatted()
    {
        var num = NextNumber.ToString().PadLeft(Padding, '0');
        return $"{Prefix}{num}";
    }

    public string Consume(string updatedBy)
    {
        if (!IsActive)
            throw new InvalidOperationException($"La serie '{Code}' está inactiva.");

        var formatted = GetNextFormatted();
        NextNumber++;
        SetUpdated(updatedBy);
        return formatted;
    }

    public void Update(string name, string prefix, int padding, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("El prefijo no puede estar vacío.", nameof(prefix));
        if (padding is < 1 or > 10)
            throw new ArgumentException("El padding debe estar entre 1 y 10.", nameof(padding));

        Name = name.Trim();
        Prefix = prefix.Trim();
        Padding = padding;
        SetUpdated(updatedBy);
    }

    public void SetNextNumber(int next, string updatedBy)
    {
        if (next < 1)
            throw new ArgumentException("El siguiente número debe ser mayor que cero.", nameof(next));
        NextNumber = next;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy) { IsActive = false; SetUpdated(updatedBy); }
    public void Activate(string updatedBy) { IsActive = true; SetUpdated(updatedBy); }
}
