using Debales.Domain.Common;

namespace Debales.Domain.Accounting;

public sealed class FiscalPeriod : Entity
{
    private FiscalPeriod() { }

    public Guid FiscalYearId { get; private set; }
    public string Name { get; private set; } = null!;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public FiscalPeriodStatus Status { get; private set; } = FiscalPeriodStatus.Open;

    internal static FiscalPeriod Create(Guid fiscalYearId, string name, DateOnly startDate, DateOnly endDate, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new FiscalPeriod
        {
            FiscalYearId = fiscalYearId,
            Name = name.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            CreatedBy = createdBy
        };
    }

    public void Close()
    {
        if (Status == FiscalPeriodStatus.Closed)
            throw new InvalidOperationException("El período ya está cerrado.");
        Status = FiscalPeriodStatus.Closed;
    }
}
