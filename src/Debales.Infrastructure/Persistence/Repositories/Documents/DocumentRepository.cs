using Debales.Application.Common;
using Debales.Application.Documents;
using Debales.Domain.Documents;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Documents;

internal sealed class DocumentRepository : BaseRepository<Document>, IDocumentRepository
{
    public DocumentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PagedResult<Document>> SearchAsync(
        string? search,
        Guid? documentTypeId,
        Guid? customerId,
        Guid? supplierId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(d =>
                d.Title.ToLower().Contains(term) ||
                (d.FileName != null && d.FileName.ToLower().Contains(term)));
        }

        if (documentTypeId.HasValue)
            query = query.Where(d => d.DocumentTypeId == documentTypeId.Value);

        if (customerId.HasValue)
            query = query.Where(d => d.CustomerId == customerId.Value);

        if (supplierId.HasValue)
            query = query.Where(d => d.SupplierId == supplierId.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(d => d.DocumentDate)
            .ThenByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Document>(items, total, page, pageSize);
    }
}
