using Debales.Domain.Common;

namespace Debales.Domain.Licensing;

public sealed class License : Entity
{
    public string InstallationId { get; private set; } = string.Empty;
    public Guid PlanId { get; private set; }
    public SubscriptionPlan Plan { get; private set; } = null!;
    public string LicenseeCompany { get; private set; } = string.Empty;
    public string LicenseeEmail { get; private set; } = string.Empty;
    public string LicenseKey { get; private set; } = string.Empty;
    public LicenseStatus Status { get; private set; }
    public DateTime StartsAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? ActivatedAt { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<LicenseModule> _modules = [];
    public IReadOnlyCollection<LicenseModule> Modules => _modules.AsReadOnly();

    private License() { }

    public static License Create(
        string installationId,
        Guid planId,
        string licenseeCompany,
        string licenseeEmail,
        string licenseKey,
        DateTime startsAt,
        DateTime expiresAt,
        string createdBy)
    {
        if (string.IsNullOrWhiteSpace(installationId))
            throw new ArgumentException("El identificador de instalación no puede estar vacío.", nameof(installationId));
        if (string.IsNullOrWhiteSpace(licenseeCompany))
            throw new ArgumentException("El nombre de la empresa no puede estar vacío.", nameof(licenseeCompany));
        if (string.IsNullOrWhiteSpace(licenseKey))
            throw new ArgumentException("La clave de licencia no puede estar vacía.", nameof(licenseKey));
        if (expiresAt <= startsAt)
            throw new ArgumentException("La fecha de expiración debe ser posterior a la fecha de inicio.", nameof(expiresAt));

        return new License
        {
            InstallationId = installationId.Trim(),
            PlanId = planId,
            LicenseeCompany = licenseeCompany.Trim(),
            LicenseeEmail = licenseeEmail.Trim().ToLowerInvariant(),
            LicenseKey = licenseKey.Trim().ToUpper(),
            Status = LicenseStatus.Trial,
            StartsAt = startsAt,
            ExpiresAt = expiresAt,
            CreatedBy = createdBy
        };
    }

    public void Activate(string updatedBy)
    {
        if (Status == LicenseStatus.Expired || Status == LicenseStatus.Suspended)
            throw new InvalidOperationException($"No se puede activar una licencia en estado '{Status}'.");

        Status = LicenseStatus.Active;
        ActivatedAt = DateTime.UtcNow;
        SetUpdated(updatedBy);
    }

    public void Suspend(string notes, string updatedBy)
    {
        Status = LicenseStatus.Suspended;
        Notes = notes?.Trim();
        SetUpdated(updatedBy);
    }

    public void AddModule(string moduleCode, string grantedBy)
    {
        if (_modules.Any(m => m.ModuleCode == moduleCode))
            return;

        _modules.Add(LicenseModule.Create(Id, moduleCode, grantedBy));
    }

    public bool IsValid() =>
        (Status == LicenseStatus.Active || Status == LicenseStatus.Trial)
        && DateTime.UtcNow >= StartsAt
        && DateTime.UtcNow <= ExpiresAt;

    public bool HasModule(string moduleCode) =>
        IsValid() && _modules.Any(m => m.ModuleCode == moduleCode);

    public void CheckAndExpire()
    {
        if (Status == LicenseStatus.Active && DateTime.UtcNow > ExpiresAt)
        {
            Status = LicenseStatus.Expired;
        }
    }
}
