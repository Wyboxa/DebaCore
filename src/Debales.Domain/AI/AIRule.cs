using Debales.Domain.Common;

namespace Debales.Domain.AI;

public sealed class AIRule : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string ActionType { get; private set; } = default!;
    public bool RequiresApproval { get; private set; }
    public bool IsActive { get; private set; }

    private AIRule() { }

    public static AIRule Create(string name, string actionType, string? description, bool requiresApproval, string? createdBy)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        if (string.IsNullOrWhiteSpace(actionType)) throw new ArgumentException("El tipo de acción es obligatorio.", nameof(actionType));

        return new AIRule
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            ActionType = actionType.Trim(),
            Description = description?.Trim(),
            RequiresApproval = requiresApproval,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Update(string name, string actionType, string? description, bool requiresApproval, string? updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        if (string.IsNullOrWhiteSpace(actionType)) throw new ArgumentException("El tipo de acción es obligatorio.", nameof(actionType));

        Name = name.Trim();
        ActionType = actionType.Trim();
        Description = description?.Trim();
        RequiresApproval = requiresApproval;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string? updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
