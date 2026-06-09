using Debales.Application.Common;
using Debales.Domain.Sales;

namespace Debales.Application.Sales;

public interface ISalesQuoteRepository : IRepository<SalesQuote>
{
    new Task<SalesQuote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SalesQuote>> SearchAsync(string? search, Guid? customerId, SalesQuoteStatus? status, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
}
