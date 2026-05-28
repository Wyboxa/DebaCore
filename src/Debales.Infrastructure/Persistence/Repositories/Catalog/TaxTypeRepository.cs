using Debales.Application.Catalog;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class TaxTypeRepository : BaseRepository<TaxType>, ITaxTypeRepository
{
    public TaxTypeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<TaxType>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet.Where(t => t.IsActive).OrderBy(t => t.Code).ToListAsync(cancellationToken);
}
