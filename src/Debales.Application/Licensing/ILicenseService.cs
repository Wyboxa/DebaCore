using Debales.Domain.Licensing;

namespace Debales.Application.Licensing;

public interface ILicenseService
{
    Task<License?> GetCurrentLicenseAsync(CancellationToken cancellationToken = default);
    Task<bool> IsValidAsync(CancellationToken cancellationToken = default);
    Task<bool> IsModuleActiveAsync(string moduleCode, CancellationToken cancellationToken = default);
}
