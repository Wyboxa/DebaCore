using Debales.Application.Common;
using Debales.Domain.Licensing;

namespace Debales.Application.Licensing;

public interface ILicenseRepository : IRepository<License>
{
    Task<License?> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task<License?> GetByKeyAsync(string licenseKey, CancellationToken cancellationToken = default);
    Task<bool> ExistsByInstallationIdAsync(string installationId, CancellationToken cancellationToken = default);
}
