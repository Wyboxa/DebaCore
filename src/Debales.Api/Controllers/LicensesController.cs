using Debales.Application.Licensing.Commands.ActivateLicense;
using Debales.Application.Licensing.Queries.GetCurrentLicense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class LicensesController : ControllerBase
{
    private readonly GetCurrentLicenseHandler _getCurrent;
    private readonly ActivateLicenseHandler _activate;

    public LicensesController(
        GetCurrentLicenseHandler getCurrent,
        ActivateLicenseHandler activate)
    {
        _getCurrent = getCurrent;
        _activate = activate;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken ct = default)
    {
        var license = await _getCurrent.Handle(ct);
        return license is null ? NotFound() : Ok(license);
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Activate([FromBody] ActivateLicenseRequest request, CancellationToken ct = default)
    {
        var license = await _activate.Handle(
            new ActivateLicenseCommand(
                request.LicenseKey,
                request.InstallationId,
                request.LicenseeCompany,
                request.LicenseeEmail,
                request.PlanCode,
                request.StartsAt,
                request.ExpiresAt,
                request.ModuleCodes,
                "api"),
            ct);
        return Ok(license);
    }

    public sealed record ActivateLicenseRequest(
        string LicenseKey,
        string InstallationId,
        string LicenseeCompany,
        string LicenseeEmail,
        string PlanCode,
        DateTime StartsAt,
        DateTime ExpiresAt,
        IReadOnlyList<string> ModuleCodes);
}
