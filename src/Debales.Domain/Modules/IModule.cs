namespace Debales.Domain.Modules;

public interface IModule
{
    string Name { get; }
    string Version { get; }
    IReadOnlyList<string> Dependencies { get; }
    IReadOnlyList<string> Permissions { get; }
}
