using Debales.Application.Licensing.DTOs;

namespace Debales.Application.Licensing.Queries.GetCurrentLicense;

public sealed class GetCurrentLicenseHandler
{
    private readonly ILicenseRepository _licenses;

    public GetCurrentLicenseHandler(ILicenseRepository licenses)
    {
        _licenses = licenses;
    }

    public async Task<LicenseSummaryDto?> Handle(CancellationToken cancellationToken = default)
    {
        var license = await _licenses.GetCurrentAsync(cancellationToken);
        if (license is null) return null;

        license.CheckAndExpire();

        return ToDto(license);
    }

    internal static LicenseSummaryDto ToDto(Domain.Licensing.License license)
    {
        var planDto = new SubscriptionPlanDto(
            license.Plan.Id,
            license.Plan.Code,
            license.Plan.Name,
            license.Plan.Description,
            license.Plan.MaxUsers,
            license.Plan.MaxModules,
            license.Plan.AllowsAI,
            license.Plan.PriceMonthly,
            license.Plan.IsActive);

        var modules = license.Modules
            .Select(m => new LicenseModuleDto(m.Id, m.ModuleCode, m.GrantedAt))
            .ToList();

        var daysRemaining = (int)Math.Max(0, (license.ExpiresAt - DateTime.UtcNow).TotalDays);

        return new LicenseSummaryDto(
            license.Id,
            license.InstallationId,
            license.LicenseeCompany,
            license.LicenseeEmail,
            license.LicenseKey,
            license.Status,
            license.StartsAt,
            license.ExpiresAt,
            license.ActivatedAt,
            license.IsValid(),
            daysRemaining,
            planDto,
            modules);
    }
}
