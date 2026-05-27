using Debales.Application.Common;
using Debales.Application.CRM.Customers;
using Debales.Domain.CRM.Customers;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.CRM;

internal sealed class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(c => c.Contacts.Where(ct => ct.IsActive))
            .Include(c => c.Activities.OrderByDescending(a => a.ScheduledAt).Take(10))
            .Include(c => c.Opportunities)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<PagedResult<Customer>> SearchAsync(string? search, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(term) ||
                (c.TaxId != null && c.TaxId.ToLower().Contains(term)) ||
                (c.Sector != null && c.Sector.ToLower().Contains(term)));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Customer>(items, total, page, pageSize);
    }

    public async Task<bool> ExistsByTaxIdAsync(string taxId, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(c => c.TaxId == taxId, cancellationToken);
}
