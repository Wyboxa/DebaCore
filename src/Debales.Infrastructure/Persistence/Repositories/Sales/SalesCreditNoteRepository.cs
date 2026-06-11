using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Sales;

internal sealed class SalesCreditNoteRepository : BaseRepository<SalesCreditNote>, ISalesCreditNoteRepository
{
    public SalesCreditNoteRepository(ApplicationDbContext context) : base(context) { }

    public new async Task<SalesCreditNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(n => n.Customer)
            .Include(n => n.OriginalInvoice)
            .Include(n => n.Lines)
            .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public async Task<PagedResult<SalesCreditNote>> SearchAsync(
        string? search, Guid? customerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(n => n.Customer)
            .Include(n => n.OriginalInvoice)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(n =>
                n.Number.ToLower().Contains(term) ||
                n.Customer!.Name.ToLower().Contains(term));
        }

        if (customerId.HasValue) query = query.Where(n => n.CustomerId == customerId);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(n => n.Date)
            .ThenByDescending(n => n.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<SalesCreditNote>(items, total, page, pageSize);
    }

    public async Task<IReadOnlyList<SalesCreditNote>> GetByCustomerForStatementAsync(
        Guid customerId, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(n => n.Lines).Where(n => n.CustomerId == customerId);
        if (from.HasValue) query = query.Where(n => n.Date >= from.Value);
        if (to.HasValue) query = query.Where(n => n.Date <= to.Value);
        return await query.OrderBy(n => n.Date).ThenBy(n => n.Number).ToListAsync(cancellationToken);
    }
}
