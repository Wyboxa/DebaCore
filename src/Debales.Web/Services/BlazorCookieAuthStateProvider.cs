using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Debales.Web.Services;

// Lee el ClaimsPrincipal del HttpContext en el momento de inicialización del circuito Blazor Server.
// A partir de ahí, el estado queda fijo hasta que el usuario recarga la página (login/logout fuerzan recarga).
public sealed class BlazorCookieAuthStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _authState;

    public BlazorCookieAuthStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User
                   ?? new ClaimsPrincipal(new ClaimsIdentity());
        _authState = new AuthenticationState(user);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(_authState);
}
