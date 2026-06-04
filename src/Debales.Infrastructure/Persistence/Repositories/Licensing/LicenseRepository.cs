using Debales.Application.Licensing;
using Debales.Domain.Licensing;
using Microsoft.EntityFrameworkCore;

namespace Debales.Infrastructure.Persistence.Repositories.Licensing;

internal sealed class LicenseRepository : BaseRepository<License>, ILicenseRepository
{
    public LicenseRepository(ApplicationDbContext context) : base(context) { }

    public async Task<License?> GetCurrentAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(l => l.Plan)
            .Include(l => l.Modules)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<License?> GetByKeyAsync(string licenseKey, CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(l => l.Plan)
            .Include(l => l.Modules)
            .FirstOrDefaultAsync(l => l.LicenseKey == licenseKey, cancellationToken);

    public async Task<bool> ExistsByInstallationIdAsync(string installationId, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(l => l.InstallationId == installationId, cancellationToken);
}
