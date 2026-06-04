using Debales.Application.Common;
using Debales.Application.Licensing.DTOs;
using Debales.Application.Licensing.Queries.GetCurrentLicense;
using Debales.Domain.Licensing;

namespace Debales.Application.Licensing.Commands.ActivateLicense;

public sealed class ActivateLicenseHandler
{
    private readonly ILicenseRepository _licenses;
    private readonly ISubscriptionPlanRepository _plans;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateLicenseHandler(
        ILicenseRepository licenses,
        ISubscriptionPlanRepository plans,
        IUnitOfWork unitOfWork)
    {
        _licenses = licenses;
        _plans = plans;
        _unitOfWork = unitOfWork;
    }

    public async Task<LicenseSummaryDto> Handle(ActivateLicenseCommand command, CancellationToken cancellationToken = default)
    {
        if (await _licenses.ExistsByInstallationIdAsync(command.InstallationId, cancellationToken))
            throw new InvalidOperationException($"Ya existe una licencia activa para la instalación '{command.InstallationId}'.");

        var plan = await _plans.GetByCodeAsync(command.PlanCode, cancellationToken)
            ?? throw new InvalidOperationException($"Plan de suscripción '{command.PlanCode}' no encontrado.");

        if (!plan.IsActive)
            throw new InvalidOperationException($"El plan '{command.PlanCode}' no está disponible.");

        var license = License.Create(
            command.InstallationId,
            plan.Id,
            command.LicenseeCompany,
            command.LicenseeEmail,
            command.LicenseKey,
            command.StartsAt,
            command.ExpiresAt,
            command.CreatedBy);

        foreach (var moduleCode in command.ModuleCodes)
            license.AddModule(moduleCode, command.CreatedBy);

        license.Activate(command.CreatedBy);

        await _licenses.AddAsync(license, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var saved = await _licenses.GetByKeyAsync(license.LicenseKey, cancellationToken)
            ?? throw new InvalidOperationException("Error al recuperar la licencia recién creada.");

        return GetCurrentLicenseHandler.ToDto(saved);
    }
}
