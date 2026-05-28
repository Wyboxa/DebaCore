namespace Debales.AI;

public sealed class AISettings
{
    public string Provider { get; init; } = "Claude";
    public string ApiKey { get; init; } = string.Empty;
    public string Model { get; init; } = "claude-sonnet-4-6";
}
