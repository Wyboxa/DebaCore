using Debales.Application.Accounting;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Accounting;

internal sealed class AccountingTemplateRepository : IAccountingTemplateRepository
{
    private readonly ApplicationDbContext _db;

    public AccountingTemplateRepository(ApplicationDbContext db) => _db = db;

    public async Task<AccountingTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.AccountingTemplates
            .Include(t => t.Lines)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IReadOnlyList<AccountingTemplate>> GetAllAsync(CancellationToken ct = default) =>
        await _db.AccountingTemplates
            .Include(t => t.Lines)
            .OrderBy(t => t.Code)
            .ToListAsync(ct);

    public async Task<AccountingTemplate?> GetByEventTypeAsync(string eventType, CancellationToken ct = default) =>
        await _db.AccountingTemplates
            .Include(t => t.Lines.OrderBy(l => l.SortOrder))
            .FirstOrDefaultAsync(t => t.EventType == eventType, ct);

    public async Task AddAsync(AccountingTemplate entity, CancellationToken ct = default) =>
        await _db.AccountingTemplates.AddAsync(entity, ct);

    public void Update(AccountingTemplate entity) => _db.AccountingTemplates.Update(entity);

    public void Remove(AccountingTemplate entity) => _db.AccountingTemplates.Remove(entity);
}
