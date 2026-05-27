using Debales.Application.Common;
using Debales.Domain.CRM.Contacts;

namespace Debales.Application.CRM.Contacts;

public interface IContactRepository : IRepository<Contact>
{
    Task<IReadOnlyList<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
}
