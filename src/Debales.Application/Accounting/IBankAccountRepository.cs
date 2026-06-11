using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IBankAccountRepository : IRepository<BankAccount>
{
    new Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<BankAccount>> SearchAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken ct = default);
    Task<List<BankAccount>> GetAllActiveAsync(CancellationToken ct = default);
}
