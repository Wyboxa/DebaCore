using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class PaymentTermRepository : BaseRepository<PaymentTerm>, IPaymentTermRepository
{
    public PaymentTermRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<PaymentTerm?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(pt => pt.Lines)
            .FirstOrDefaultAsync(pt => pt.Id == id, cancellationToken);

    public async Task<PagedResult<PaymentTerm>> SearchAsync(
        string? search, bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(pt => pt.Lines).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(pt => pt.Name.ToLower().Contains(term));
        }

        if (isActive.HasValue) query = query.Where(pt => pt.IsActive == isActive);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(pt => pt.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PaymentTerm>(items, total, page, pageSize);
    }

    public async Task<List<PaymentTerm>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(pt => pt.Lines)
            .Where(pt => pt.IsActive)
            .OrderBy(pt => pt.Name)
            .ToListAsync(cancellationToken);
}
