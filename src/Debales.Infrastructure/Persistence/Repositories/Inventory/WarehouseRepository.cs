using Debales.Application.Inventory;
using Debales.Domain.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Inventory;

internal sealed class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(w => w.Locations)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Warehouse>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet.Where(w => w.IsActive).OrderBy(w => w.Name).ToListAsync(cancellationToken);

    public new async Task<IReadOnlyList<Warehouse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.OrderBy(w => w.Name).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(w => w.Code == code.Trim().ToUpper(), cancellationToken);
}
