namespace Debales.Application.Licensing.Commands.ActivateLicense;

public sealed record ActivateLicenseCommand(
    string LicenseKey,
    string InstallationId,
    string LicenseeCompany,
    string LicenseeEmail,
    string PlanCode,
    DateTime StartsAt,
    DateTime ExpiresAt,
    IReadOnlyList<string> ModuleCodes,
    string CreatedBy);
