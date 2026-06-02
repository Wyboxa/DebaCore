using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class FiscalYear : AuditableEntity
{
    private readonly List<FiscalPeriod> _periods = [];
    private FiscalYear() { }

    public string Name { get; private set; } = null!;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public FiscalYearStatus Status { get; private set; } = FiscalYearStatus.Open;

    public IReadOnlyList<FiscalPeriod> Periods => _periods.AsReadOnly();

    public static FiscalYear Create(string name, DateOnly startDate, DateOnly endDate, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (endDate <= startDate)
            throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");

        return new FiscalYear
        {
            Name = name.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            CreatedBy = createdBy
        };
    }

    public FiscalPeriod AddPeriod(string periodName, DateOnly startDate, DateOnly endDate, string createdBy)
    {
        if (Status == FiscalYearStatus.Closed)
            throw new InvalidOperationException("No se pueden añadir períodos a un ejercicio cerrado.");
        if (startDate < StartDate || endDate > EndDate)
            throw new ArgumentException("Las fechas del período deben estar dentro del ejercicio fiscal.");
        if (endDate <= startDate)
            throw new ArgumentException("La fecha de fin del período debe ser posterior a la de inicio.");

        var period = FiscalPeriod.Create(Id, periodName, startDate, endDate, createdBy);
        _periods.Add(period);
        return period;
    }

    public void Close(string updatedBy)
    {
        if (Status == FiscalYearStatus.Closed)
            throw new InvalidOperationException("El ejercicio ya está cerrado.");
        Status = FiscalYearStatus.Closed;
        SetUpdated(updatedBy);
    }
}
