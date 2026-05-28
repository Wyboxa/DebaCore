using Debales.Application.Catalog;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class ItemFamilyRepository : BaseRepository<ItemFamily>, IItemFamilyRepository
{
    public ItemFamilyRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<ItemFamily>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet.Where(f => f.IsActive).OrderBy(f => f.Name).ToListAsync(cancellationToken);
}
