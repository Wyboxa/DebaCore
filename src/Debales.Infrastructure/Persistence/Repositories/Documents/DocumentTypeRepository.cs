using Debales.Application.Documents;
using Debales.Domain.Documents;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Documents;

internal sealed class DocumentTypeRepository : BaseRepository<DocumentType>, IDocumentTypeRepository
{
    public DocumentTypeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<DocumentType>> GetAllActiveAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
}
