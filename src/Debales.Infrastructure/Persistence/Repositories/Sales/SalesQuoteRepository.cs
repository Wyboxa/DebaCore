using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class SalesQuoteRepository : BaseRepository<SalesQuote>, ISalesQuoteRepository
{
    public SalesQuoteRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<SalesQuote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(q => q.Customer)
            .Include(q => q.Lines)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);

    public async Task<PagedResult<SalesQuote>> SearchAsync(
        string? search, Guid? customerId, SalesQuoteStatus? status,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(q => q.Customer)
            .Include(q => q.Lines)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(q =>
                q.Number.ToLower().Contains(term) ||
                q.Customer!.Name.ToLower().Contains(term));
        }

        if (customerId.HasValue)
            query = query.Where(q => q.CustomerId == customerId);

        if (status.HasValue)
            query = query.Where(q => q.Status == status);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(q => q.Date)
            .ThenByDescending(q => q.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SalesQuote>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(q => q.Number == number.Trim().ToUpper(), cancellationToken);

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"PRE-{year}-";
        var count = await Context.Set<SalesQuote>()
            .IgnoreQueryFilters()
            .CountAsync(q => q.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
