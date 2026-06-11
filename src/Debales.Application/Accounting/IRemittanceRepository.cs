using Debales.Application.Common;
using Debales.Domain.Accounting;

namespace Debales.Application.Accounting;

public interface IRemittanceRepository : IRepository<Remittance>
{
    new Task<Remittance?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<Remittance>> SearchAsync(RemittanceType? type, RemittanceStatus? status, int page, int pageSize, CancellationToken ct = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken ct = default);
    Task<string> GetNextNumberAsync(RemittanceType type, CancellationToken ct = default);
}
