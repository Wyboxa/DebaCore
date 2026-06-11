using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class ReceivableRepository : BaseRepository<Receivable>, IReceivableRepository
{
    public ReceivableRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<Receivable?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(r => r.SalesInvoice)
            .Include(r => r.Customer)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Receivable>> GetByInvoiceAsync(Guid salesInvoiceId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(r => r.Customer)
            .Where(r => r.SalesInvoiceId == salesInvoiceId)
            .ToListAsync(cancellationToken);

    public async Task<PagedResult<Receivable>> SearchAsync(
        string? search, Guid? customerId, ReceivableStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(r => r.SalesInvoice)
            .Include(r => r.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(r =>
                r.Number.ToLower().Contains(term) ||
                r.Customer!.Name.ToLower().Contains(term) ||
                r.SalesInvoice!.Number.ToLower().Contains(term));
        }

        if (customerId.HasValue) query = query.Where(r => r.CustomerId == customerId);
        if (status.HasValue) query = query.Where(r => r.Status == status);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(r => r.DueDate)
            .ThenByDescending(r => r.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Receivable>(items, total, page, pageSize);
    }

    public async Task<IReadOnlyList<Receivable>> GetForAgingAsync(Guid? customerId = null, CancellationToken ct = default)
    {
        var query = DbSet.Include(r => r.Customer).AsQueryable();
        query = query.Where(r => r.Status == ReceivableStatus.Pending || r.Status == ReceivableStatus.Partial);
        if (customerId.HasValue) query = query.Where(r => r.CustomerId == customerId);
        return await query.OrderBy(r => r.DueDate).ToListAsync(ct);
    }

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"VTO-{year}-";
        var count = await Context.Set<Receivable>()
            .IgnoreQueryFilters()
            .CountAsync(r => r.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
