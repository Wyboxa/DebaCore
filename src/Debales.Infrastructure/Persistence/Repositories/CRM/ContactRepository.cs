using Debales.Application.CRM.Contacts;
using Debales.Domain.CRM.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.CRM;

internal sealed class ContactRepository : BaseRepository<Contact>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(c => c.CustomerId == customerId)
            .OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
            .ToListAsync(cancellationToken);
}
