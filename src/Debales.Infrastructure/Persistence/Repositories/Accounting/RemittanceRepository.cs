using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class RemittanceRepository : BaseRepository<Remittance>, IRemittanceRepository
{
    public RemittanceRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<Remittance?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await DbSet
            .Include(r => r.BankAccount)
            .Include(r => r.Lines)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<PagedResult<Remittance>> SearchAsync(
        RemittanceType? type, RemittanceStatus? status,
        int page, int pageSize, CancellationToken ct = default)
    {
        var query = DbSet.Include(r => r.BankAccount).AsQueryable();

        if (type.HasValue) query = query.Where(r => r.Type == type);
        if (status.HasValue) query = query.Where(r => r.Status == status);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Remittance>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken ct = default) =>
        await DbSet.AnyAsync(r => r.Number == number.Trim().ToUpperInvariant(), ct);

    public async Task<string> GetNextNumberAsync(RemittanceType type, CancellationToken ct = default)
    {
        var prefix = type == RemittanceType.Collection ? "REM-C" : "REM-P";
        var year = DateTime.Today.Year;
        var count = await DbSet
            .Where(r => r.Number.StartsWith($"{prefix}-{year}"))
            .CountAsync(ct);
        return $"{prefix}-{year}-{count + 1:D4}";
    }
}
