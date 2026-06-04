using Debales.Application.Licensing.Queries.GetSubscriptionPlans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Debales.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SubscriptionPlansController : ControllerBase
{
    private readonly GetSubscriptionPlansHandler _getAll;

    public SubscriptionPlansController(GetSubscriptionPlansHandler getAll)
    {
        _getAll = getAll;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var plans = await _getAll.Handle(ct);
        return Ok(plans);
    }
}
