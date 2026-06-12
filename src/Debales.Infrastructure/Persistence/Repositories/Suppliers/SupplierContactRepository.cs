using Debales.Application.Suppliers.Contacts;
using Debales.Domain.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Suppliers;

internal sealed class SupplierContactRepository : BaseRepository<SupplierContact>, ISupplierContactRepository
{
    public SupplierContactRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<SupplierContact>> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(c => c.SupplierId == supplierId)
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
            .ToListAsync(cancellationToken);
}
