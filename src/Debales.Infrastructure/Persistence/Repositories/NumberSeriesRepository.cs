using Debales.Application.Core.NumberSeries;
using Debales.Domain.Core.NumberSeries;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories;

public sealed class NumberSeriesRepository : INumberSeriesRepository
{
    private readonly ApplicationDbContext _db;

    public NumberSeriesRepository(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<NumberSeries>> GetAllAsync(CancellationToken ct = default) =>
        await _db.NumberSeries.OrderBy(s => s.Code).ToListAsync(ct);

    public async Task<NumberSeries?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        await _db.NumberSeries.FirstOrDefaultAsync(s => s.Code == code.ToUpperInvariant(), ct);

    public async Task AddAsync(NumberSeries series, CancellationToken ct = default) =>
        await _db.NumberSeries.AddAsync(series, ct);

    public void Update(NumberSeries series) =>
        _db.NumberSeries.Update(series);
}
