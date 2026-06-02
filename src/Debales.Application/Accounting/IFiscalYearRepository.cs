using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IFiscalYearRepository : IRepository<FiscalYear>
{
    Task<IReadOnlyList<FiscalYear>> GetAllWithPeriodsAsync(CancellationToken ct = default);
    Task<FiscalYear?> GetByIdWithPeriodsAsync(Guid id, CancellationToken ct = default);
    Task<FiscalPeriod?> GetOpenPeriodForDateAsync(DateOnly date, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
}
