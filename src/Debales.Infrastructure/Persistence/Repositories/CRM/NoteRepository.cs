using Debales.Application.CRM.Notes;
using Debales.Domain.CRM.Notes;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.CRM;

internal sealed class NoteRepository : BaseRepository<Note>, INoteRepository
{
    public NoteRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Note>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(n => n.CustomerId == customerId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
}
