using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IAccountingTemplateRepository : IRepository<AccountingTemplate>
{
    Task<AccountingTemplate?> GetByEventTypeAsync(string eventType, CancellationToken ct = default);
}
