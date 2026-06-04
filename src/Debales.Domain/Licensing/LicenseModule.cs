using Debales.Domain.Common;

namespace Debales.Domain.Licensing;

public sealed class LicenseModule : Entity
{
    public Guid LicenseId { get; private set; }
    public string ModuleCode { get; private set; } = string.Empty;
    public DateTime GrantedAt { get; private set; }

    private LicenseModule() { }

    internal static LicenseModule Create(Guid licenseId, string moduleCode, string grantedBy)
    {
        if (string.IsNullOrWhiteSpace(moduleCode))
            throw new ArgumentException("El código de módulo no puede estar vacío.", nameof(moduleCode));

        return new LicenseModule
        {
            LicenseId = licenseId,
            ModuleCode = moduleCode.Trim().ToUpper(),
            GrantedAt = DateTime.UtcNow,
            CreatedBy = grantedBy
        };
    }
}
