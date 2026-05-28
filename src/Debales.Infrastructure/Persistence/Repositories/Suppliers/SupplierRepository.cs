using Debales.Application.Common;
using Debales.Application.Suppliers;
using Debales.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Suppliers;

internal sealed class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<PagedResult<Supplier>> SearchAsync(string? search, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s =>
                s.Name.ToLower().Contains(term) ||
                (s.TaxId != null && s.TaxId.ToLower().Contains(term)) ||
                (s.ContactName != null && s.ContactName.ToLower().Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Supplier>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(s => s.TaxId == taxId, cancellationToken);
}
