using Debales.Domain.Licensing;

namespace Debales.Application.Licensing.DTOs;

public sealed record LicenseSummaryDto(
    Guid Id,
    string InstallationId,
    string LicenseeCompany,
    string LicenseeEmail,
    string LicenseKey,
    LicenseStatus Status,
    DateTime StartsAt,
    DateTime ExpiresAt,
    DateTime? ActivatedAt,
    bool IsValid,
    int DaysRemaining,
    SubscriptionPlanDto Plan,
    IReadOnlyList<LicenseModuleDto> Modules);
