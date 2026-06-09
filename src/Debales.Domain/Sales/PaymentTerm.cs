using Debales.Domain.Common;

namespace Debales.Domain.Sales;

public sealed class PaymentTerm : AuditableEntity
{
    private readonly List<PaymentTermLine> _lines = [];

    private PaymentTerm() { }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;

    public IReadOnlyList<PaymentTermLine> Lines => _lines.AsReadOnly();

    public static PaymentTerm Create(string name, string? description,
        IEnumerable<(int DueDays, decimal Percentage)> lines, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var pt = new PaymentTerm { Name = name.Trim(), Description = description?.Trim(), CreatedBy = createdBy };
        pt.SetLines(lines);
        return pt;
    }

    public void Update(string name, string? description,
        IEnumerable<(int DueDays, decimal Percentage)> lines, string updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        Description = description?.Trim();
        SetLines(lines);
        SetUpdated(updatedBy);
    }

    public void Activate(string updatedBy) { IsActive = true; SetUpdated(updatedBy); }
    public void Deactivate(string updatedBy) { IsActive = false; SetUpdated(updatedBy); }
    public void Delete(string deletedBy) => SoftDelete(deletedBy);

    private void SetLines(IEnumerable<(int DueDays, decimal Percentage)> lines)
    {
        var list = lines.ToList();
        if (list.Count == 0) throw new ArgumentException("Se requiere al menos un plazo.", nameof(lines));
        if (list.Any(l => l.DueDays < 0)) throw new ArgumentException("Los días de vencimiento no pueden ser negativos.");
        if (list.Any(l => l.Percentage <= 0)) throw new ArgumentException("El porcentaje debe ser mayor que cero.");

        var total = list.Sum(l => l.Percentage);
        if (Math.Abs(total - 100m) > 0.01m)
            throw new ArgumentException($"Los porcentajes deben sumar 100 (suma actual: {total:F2}).");

        _lines.Clear();
        int order = 1;
        foreach (var (days, pct) in list.OrderBy(l => l.DueDays))
            _lines.Add(PaymentTermLine.Create(Id, days, pct, order++));
    }

    /// <summary>
    /// Calcula los vencimientos que generaría esta condición aplicada a un importe e fecha base.
    /// </summary>
    public IReadOnlyList<(DateOnly DueDate, decimal Amount)> Calculate(DateOnly baseDate, decimal total)
    {
        var result = new List<(DateOnly, decimal)>();
        decimal assigned = 0m;
        for (int i = 0; i < _lines.Count; i++)
        {
            var line = _lines[i];
            var dueDate = baseDate.AddDays(line.DueDays);
            decimal amount = i == _lines.Count - 1
                ? total - assigned
                : Math.Round(total * line.Percentage / 100m, 2);
            assigned += amount;
            result.Add((dueDate, amount));
        }
        return result;
    }
}
