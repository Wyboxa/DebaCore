using Debales.Application.Accounting.Commands.CreateFiscalYear;
using Debales.Application.Accounting.DTOs;

namespace Debales.Application.Accounting.Queries.GetFiscalYears;

public sealed class GetFiscalYearsHandler
{
    private readonly IFiscalYearRepository _fiscalYears;

    public GetFiscalYearsHandler(IFiscalYearRepository fiscalYears) => _fiscalYears = fiscalYears;

    public async Task<IReadOnlyList<FiscalYearDetailDto>> Handle(GetFiscalYearsQuery query, CancellationToken ct = default)
    {
        var years = await _fiscalYears.GetAllWithPeriodsAsync(ct);
        return years.Select(CreateFiscalYearHandler.ToDetailDto).ToList();
    }
}
