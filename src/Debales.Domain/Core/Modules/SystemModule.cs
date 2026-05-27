using Debales.Domain.Common;

namespace Debales.Domain.Core.Modules;

public sealed class SystemModule : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public bool IsEnabled { get; private set; }
    public string DependenciesJson { get; private set; } = "[]";

    private SystemModule() { }

    public static SystemModule Register(string name, string version, string createdBy, bool isEnabled = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del módulo no puede estar vacío.", nameof(name));

        return new SystemModule
        {
            Name = name.Trim(),
            Version = version.Trim(),
            IsEnabled = isEnabled,
            CreatedBy = createdBy
        };
    }

    public void Enable(string updatedBy)
    {
        IsEnabled = true;
        SetUpdated(updatedBy);
    }

    public void Disable(string updatedBy)
    {
        IsEnabled = false;
        SetUpdated(updatedBy);
    }
}
