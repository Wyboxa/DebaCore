using Debales.Application.Common;
using Microsoft.AspNetCore.Http;

namespace Debales.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http) => _http = http;

    public string? Username => _http.HttpContext?.User?.Identity?.Name;
}
