using Debales.Application.Common;
using Microsoft.AspNetCore.Http;

namespace Debales.Infrastructure.Services;

public sealed class HttpContextTenantService : ITenantService
{
    private readonly IHttpContextAccessor _http;

    public HttpContextTenantService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public Guid? CurrentTenantId
    {
        get
        {
            var claim = _http.HttpContext?.User.FindFirst("tenant_id")?.Value;
            return claim is not null && Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
