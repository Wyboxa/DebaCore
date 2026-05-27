using Debales.Domain.Modules;

namespace Debales.Modules.Core;

public sealed class CoreModule : IModule
{
    public string Name => "Core";
    public string Version => "1.0.0";

    public IReadOnlyList<string> Dependencies =>
        Array.Empty<string>();

    public IReadOnlyList<string> Permissions =>
    [
        "core.users.read",
        "core.users.write",
        "core.roles.read",
        "core.roles.write",
        "core.modules.read",
        "core.modules.write",
        "core.settings.read",
        "core.settings.write",
        "core.audit.read"
    ];
}
