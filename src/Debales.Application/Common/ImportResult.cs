namespace Debales.Application.Common;

public sealed record ImportResult(
    int Success,
    int Failed,
    IReadOnlyList<string> Errors)
{
    public int Total => Success + Failed;
    public bool HasErrors => Errors.Count > 0;
}
