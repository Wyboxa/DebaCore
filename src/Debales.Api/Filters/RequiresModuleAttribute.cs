using Debales.Application.Licensing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Debales.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequiresModuleAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    private readonly string _moduleCode;

    public RequiresModuleAttribute(string moduleCode) => _moduleCode = moduleCode;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var licenseService = serviceProvider.GetRequiredService<ILicenseService>();
        return new ModuleAuthorizationFilter(licenseService, _moduleCode);
    }
}

internal sealed class ModuleAuthorizationFilter : IAsyncActionFilter
{
    private readonly ILicenseService _licenseService;
    private readonly string _moduleCode;

    public ModuleAuthorizationFilter(ILicenseService licenseService, string moduleCode)
    {
        _licenseService = licenseService;
        _moduleCode = moduleCode;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // No license registered → new installation, allow all modules
        if (!await _licenseService.IsValidAsync(context.HttpContext.RequestAborted))
        {
            await next();
            return;
        }

        if (!await _licenseService.IsModuleActiveAsync(_moduleCode, context.HttpContext.RequestAborted))
        {
            context.Result = new ObjectResult(new { error = $"Módulo '{_moduleCode}' no licenciado." })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}
