using Debales.Application.Common;
using Debales.Domain.CRM.Notes;

namespace Debales.Application.CRM.Notes;

public interface INoteRepository : IRepository<Note>
{
    Task<IReadOnlyList<Note>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
