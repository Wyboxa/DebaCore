using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface ICashAccountRepository : IRepository<CashAccount>
{
    new Task<CashAccount?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<CashAccount>> SearchAsync(string? search, bool? isActive, int page, int pageSize, CancellationToken ct = default);
    Task<List<CashAccount>> GetAllActiveAsync(CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, Guid? excludeId, CancellationToken ct = default);
}
