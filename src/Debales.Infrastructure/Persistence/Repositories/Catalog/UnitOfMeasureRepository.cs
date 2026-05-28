using Debales.Application.Catalog;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class UnitOfMeasureRepository : BaseRepository<UnitOfMeasure>, IUnitOfMeasureRepository
{
    public UnitOfMeasureRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<UnitOfMeasure>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet.Where(u => u.IsActive).OrderBy(u => u.Code).ToListAsync(cancellationToken);
}
