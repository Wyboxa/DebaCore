using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class PaymentMethodRepository : BaseRepository<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<PaymentMethod?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(pm => pm.Id == id, cancellationToken);

    public async Task<PagedResult<PaymentMethod>> SearchAsync(
        string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(pm => pm.Name.ToLower().Contains(term) ||
                                     (pm.Code != null && pm.Code.ToLower().Contains(term)));
        }

        if (isActive.HasValue) query = query.Where(pm => pm.IsActive == isActive);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(pm => pm.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PaymentMethod>(items, total, page, pageSize);
    }

    public async Task<List<PaymentMethod>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(pm => pm.IsActive)
            .OrderBy(pm => pm.Name)
            .ToListAsync(cancellationToken);
}
