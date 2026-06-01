using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class CustomerPaymentRepository : BaseRepository<CustomerPayment>, ICustomerPaymentRepository
{
    public CustomerPaymentRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<CustomerPayment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(p => p.Customer)
            .Include(p => p.Receivable)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<PagedResult<CustomerPayment>> SearchAsync(
        string? search, Guid? customerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(p => p.Customer)
            .Include(p => p.Receivable)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p =>
                p.Number.ToLower().Contains(term) ||
                p.Customer!.Name.ToLower().Contains(term));
        }

        if (customerId.HasValue) query = query.Where(p => p.CustomerId == customerId);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<CustomerPayment>(items, total, page, pageSize);
    }

    public async Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"COB-{year}-";
        var count = await Context.Set<CustomerPayment>()
            .IgnoreQueryFilters()
            .CountAsync(p => p.Number.StartsWith(prefix), cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }
}
