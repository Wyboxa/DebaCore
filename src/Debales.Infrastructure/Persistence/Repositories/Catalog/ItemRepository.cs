using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class ItemRepository : BaseRepository<Item>, IItemRepository
{
    public ItemRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<Item?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(i => i.Family)
            .Include(i => i.UnitOfMeasure)
            .Include(i => i.TaxType)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<PagedResult<Item>> SearchAsync(
        string? search, Guid? familyId, bool? isService, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(i => i.Family)
            .Include(i => i.UnitOfMeasure)
            .Include(i => i.TaxType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(i =>
                i.Code.ToLower().Contains(term) ||
                i.Name.ToLower().Contains(term));
        }

        if (familyId.HasValue)
            query = query.Where(i => i.FamilyId == familyId);

        if (isService.HasValue)
            query = query.Where(i => i.IsService == isService);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(i => i.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Item>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(i => i.Code == code.Trim().ToUpper(), cancellationToken);

    public async Task<bool> ExistsByCodeAsync(string code, Guid excludeId, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(i => i.Code == code.Trim().ToUpper() && i.Id != excludeId, cancellationToken);
}
