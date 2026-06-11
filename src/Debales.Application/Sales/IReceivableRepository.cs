using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface IReceivableRepository : IRepository<Receivable>
{
    new Task<Receivable?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Receivable>> GetByInvoiceAsync(Guid salesInvoiceId, CancellationToken cancellationToken = default);
    Task<PagedResult<Receivable>> SearchAsync(string? search, Guid? customerId, ReceivableStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Receivable>> GetForAgingAsync(Guid? customerId = null, CancellationToken ct = default);
    Task<string> GetNextNumberAsync(CancellationToken cancellationToken = default);
}
