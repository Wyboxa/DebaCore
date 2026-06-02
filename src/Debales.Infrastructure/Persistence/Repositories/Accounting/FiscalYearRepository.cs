using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class FiscalYearRepository : IFiscalYearRepository
{
    private readonly ApplicationDbContext _db;

    public FiscalYearRepository(ApplicationDbContext db) => _db = db;

    public async Task<FiscalYear?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.FiscalYears.FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<IReadOnlyList<FiscalYear>> GetAllAsync(CancellationToken ct = default) =>
        await _db.FiscalYears.OrderByDescending(f => f.StartDate).ToListAsync(ct);

    public async Task<IReadOnlyList<FiscalYear>> GetAllWithPeriodsAsync(CancellationToken ct = default) =>
        await _db.FiscalYears
            .Include(f => f.Periods)
            .OrderByDescending(f => f.StartDate)
            .ToListAsync(ct);

    public async Task<FiscalYear?> GetByIdWithPeriodsAsync(Guid id, CancellationToken ct = default) =>
        await _db.FiscalYears
            .Include(f => f.Periods)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<FiscalPeriod?> GetOpenPeriodForDateAsync(DateOnly date, CancellationToken ct = default) =>
        await _db.FiscalPeriods
            .Where(p => p.Status == FiscalPeriodStatus.Open && p.StartDate <= date && p.EndDate >= date)
            .FirstOrDefaultAsync(ct);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
        await _db.FiscalYears.AnyAsync(f => f.Name == name, ct);

    public async Task AddAsync(FiscalYear entity, CancellationToken ct = default) =>
        await _db.FiscalYears.AddAsync(entity, ct);

    public void Update(FiscalYear entity) => _db.FiscalYears.Update(entity);

    public void Remove(FiscalYear entity) => _db.FiscalYears.Remove(entity);
}
