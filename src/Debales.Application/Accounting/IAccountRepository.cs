using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IAccountRepository : IRepository<Account>
{
    Task<PagedResult<Account>> SearchAsync(string? search, int page, int pageSize, CancellationToken ct = default);
    Task<Account?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
}
