using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface ISalesCreditNoteRepository : IRepository<SalesCreditNote>
{
    new Task<SalesCreditNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SalesCreditNote>> SearchAsync(string? search, Guid? customerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SalesCreditNote>> GetByCustomerForStatementAsync(Guid customerId, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default);
}
