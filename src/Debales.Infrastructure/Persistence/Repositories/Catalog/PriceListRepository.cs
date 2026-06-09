using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class PriceListRepository : BaseRepository<PriceList>, IPriceListRepository
{
    public PriceListRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<PriceList?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(pl => pl.Items)
                .ThenInclude(ip => ip.Item)
                    .ThenInclude(i => i.UnitOfMeasure)
            .FirstOrDefaultAsync(pl => pl.Id == id, cancellationToken);

    public async Task<PagedResult<PriceList>> SearchAsync(
        string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(pl => pl.Items).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(pl => pl.Name.ToLower().Contains(term));
        }

        if (isActive.HasValue) query = query.Where(pl => pl.IsActive == isActive);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(pl => pl.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PriceList>(items, total, page, pageSize);
    }

    public async Task<List<PriceList>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(pl => pl.IsActive)
            .OrderBy(pl => pl.Name)
            .ToListAsync(cancellationToken);
}
