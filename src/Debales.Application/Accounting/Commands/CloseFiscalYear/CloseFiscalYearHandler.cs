using Debales.Application.Common;

namespace Debales.Application.Accounting.Commands.CloseFiscalYear;

public sealed class CloseFiscalYearHandler
{
    private readonly IFiscalYearRepository _fiscalYears;
    private readonly IUnitOfWork _unitOfWork;

    public CloseFiscalYearHandler(IFiscalYearRepository fiscalYears, IUnitOfWork unitOfWork)
    {
        _fiscalYears = fiscalYears;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CloseFiscalYearCommand command, CancellationToken cancellationToken = default)
    {
        var year = await _fiscalYears.GetByIdWithPeriodsAsync(command.YearId, cancellationToken)
            ?? throw new KeyNotFoundException($"Ejercicio fiscal '{command.YearId}' no encontrado.");

        year.Close(command.UpdatedBy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
