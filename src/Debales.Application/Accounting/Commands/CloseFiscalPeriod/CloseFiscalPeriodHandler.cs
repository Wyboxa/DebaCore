using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.CloseFiscalPeriod;

public sealed class CloseFiscalPeriodHandler
{
    private readonly IFiscalYearRepository _fiscalYears;
    private readonly IUnitOfWork _unitOfWork;

    public CloseFiscalPeriodHandler(IFiscalYearRepository fiscalYears, IUnitOfWork unitOfWork)
    {
        _fiscalYears = fiscalYears;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CloseFiscalPeriodCommand command, CancellationToken cancellationToken = default)
    {
        var years = await _fiscalYears.GetAllWithPeriodsAsync(cancellationToken);
        var period = years
            .SelectMany(y => y.Periods)
            .FirstOrDefault(p => p.Id == command.PeriodId)
            ?? throw new KeyNotFoundException($"Período '{command.PeriodId}' no encontrado.");

        period.Close();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
