using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Catalog;

internal sealed class CustomerItemCodeRepository : BaseRepository<CustomerItemCode>, ICustomerItemCodeRepository
{
    public CustomerItemCodeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<CustomerItemCode>> GetByCustomerIdAsync(
        Guid customerId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(c => c.Item).ThenInclude(i => i.UnitOfMeasure)
            .Where(c => c.CustomerId == customerId)
            .OrderBy(c => c.Item.Code)
            .ToListAsync(cancellationToken);

    public async Task<CustomerItemCode?> GetByCustomerAndItemAsync(
        Guid customerId, Guid itemId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(c => c.Item)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.ItemId == itemId, cancellationToken);
}
