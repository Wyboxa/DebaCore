using Debales.Application.Licensing;
using Debales.Domain.Licensing;

namespace Debales.Infrastructure.Services;

internal sealed class LicenseService : ILicenseService
{
    private readonly ILicenseRepository _licenses;

    public LicenseService(ILicenseRepository licenses)
    {
        _licenses = licenses;
    }

    public async Task<License?> GetCurrentLicenseAsync(CancellationToken cancellationToken = default) =>
        await _licenses.GetCurrentAsync(cancellationToken);

    public async Task<bool> IsValidAsync(CancellationToken cancellationToken = default)
    {
        var license = await _licenses.GetCurrentAsync(cancellationToken);
        return license?.IsValid() ?? false;
    }

    public async Task<bool> IsModuleActiveAsync(string moduleCode, CancellationToken cancellationToken = default)
    {
        var license = await _licenses.GetCurrentAsync(cancellationToken);
        return license?.HasModule(moduleCode) ?? false;
    }
}
