using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class SupplierItemCodeRepository : BaseRepository<SupplierItemCode>, ISupplierItemCodeRepository
{
    public SupplierItemCodeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<SupplierItemCode>> GetBySupplierIdAsync(
        Guid supplierId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(c => c.Item).ThenInclude(i => i.UnitOfMeasure)
            .Where(c => c.SupplierId == supplierId)
            .OrderBy(c => c.Item.Code)
            .ToListAsync(cancellationToken);

    public async Task<SupplierItemCode?> GetBySupplierAndItemAsync(
        Guid supplierId, Guid itemId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(c => c.Item)
            .FirstOrDefaultAsync(c => c.SupplierId == supplierId && c.ItemId == itemId, cancellationToken);
}
