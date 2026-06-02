using Debales.Application.Accounting.DTOs;
using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting.Commands.CreateFiscalYear;

public sealed class CreateFiscalYearHandler
{
    private readonly IFiscalYearRepository _fiscalYears;
    private readonly IUnitOfWork _uow;

    public CreateFiscalYearHandler(IFiscalYearRepository fiscalYears, IUnitOfWork uow)
    {
        _fiscalYears = fiscalYears;
        _uow = uow;
    }

    public async Task<FiscalYearSummaryDto> Handle(CreateFiscalYearCommand command, CancellationToken ct = default)
    {
        if (await _fiscalYears.ExistsByNameAsync(command.Name, ct))
            throw new InvalidOperationException($"Ya existe un ejercicio fiscal con el nombre '{command.Name}'.");

        var year = FiscalYear.Create(command.Name, command.StartDate, command.EndDate, command.CreatedBy);

        // Generar los 12 períodos mensuales automáticamente
        var current = command.StartDate;
        while (current <= command.EndDate)
        {
            var monthEnd = new DateOnly(current.Year, current.Month, DateTime.DaysInMonth(current.Year, current.Month));
            if (monthEnd > command.EndDate) monthEnd = command.EndDate;
            var periodName = $"{current:MMMM yyyy}";
            year.AddPeriod(periodName, current, monthEnd, command.CreatedBy);
            current = monthEnd.AddDays(1);
        }

        await _fiscalYears.AddAsync(year, ct);
        await _uow.SaveChangesAsync(ct);

        return ToSummaryDto(year);
    }

    internal static FiscalYearSummaryDto ToSummaryDto(FiscalYear y) => new(
        y.Id, y.Name, y.StartDate, y.EndDate, y.Status, StatusLabel(y.Status), y.Periods.Count);

    internal static FiscalYearDetailDto ToDetailDto(FiscalYear y) => new(
        y.Id, y.Name, y.StartDate, y.EndDate, y.Status, StatusLabel(y.Status),
        y.Periods.Select(ToPeriodDto).ToList(),
        y.CreatedAt, y.CreatedBy);

    internal static FiscalPeriodDto ToPeriodDto(FiscalPeriod p) => new(
        p.Id, p.FiscalYearId, p.Name, p.StartDate, p.EndDate,
        p.Status, p.Status == FiscalPeriodStatus.Open ? "Abierto" : "Cerrado");

    private static string StatusLabel(FiscalYearStatus s) => s switch
    {
        FiscalYearStatus.Open => "Abierto",
        FiscalYearStatus.Closed => "Cerrado",
        _ => s.ToString()
    };
}
