using Debales.Domain.Common;

namespace Debales.Domain.Core.Audit;

public sealed class AuditEntry : Entity
{
    public string EntityName { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string? UserId { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    private AuditEntry() { }

    public static AuditEntry Record(
        string entityName,
        string entityId,
        string action,
        string? userId = null,
        string? oldValues = null,
        string? newValues = null)
    {
        return new AuditEntry
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            UserId = userId,
            OldValues = oldValues,
            NewValues = newValues,
            CreatedBy = userId ?? "system"
        };
    }
}
