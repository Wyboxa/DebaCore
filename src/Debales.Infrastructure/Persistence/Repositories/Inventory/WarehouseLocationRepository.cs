using Debales.Application.Inventory;
using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Inventory;

internal sealed class WarehouseLocationRepository : BaseRepository<WarehouseLocation>, IWarehouseLocationRepository
{
    public WarehouseLocationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<WarehouseLocation>> GetByWarehouseAsync(Guid warehouseId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(l => l.WarehouseId == warehouseId)
            .OrderBy(l => l.Code)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByCodeAsync(Guid warehouseId, string code, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(l => l.WarehouseId == warehouseId && l.Code == code.Trim().ToUpper(), cancellationToken);
}
