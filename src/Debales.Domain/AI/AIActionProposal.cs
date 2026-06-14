using Debales.Domain.Common;

namespace Debales.Domain.AI;

public sealed class AIActionProposal : AuditableEntity
{
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string ActionType { get; private set; } = default!;
    public string? TargetEntity { get; private set; }
    public Guid? TargetEntityId { get; private set; }
    public string Payload { get; private set; } = "{}";
    public AIProposalStatus Status { get; private set; }
    public string? ProposedByModel { get; private set; }
    public DateTime ProposedAt { get; private set; }
    public string? RejectionReason { get; private set; }

    private AIActionProposal() { }

    public static AIActionProposal Create(
        string title,
        string actionType,
        string? payload = null,
        string? description = null,
        string? targetEntity = null,
        Guid? targetEntityId = null,
        string? proposedByModel = null,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El título es obligatorio.", nameof(title));
        if (string.IsNullOrWhiteSpace(actionType)) throw new ArgumentException("El tipo de acción es obligatorio.", nameof(actionType));

        return new AIActionProposal
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description?.Trim(),
            ActionType = actionType.Trim(),
            TargetEntity = targetEntity?.Trim(),
            TargetEntityId = targetEntityId,
            Payload = string.IsNullOrWhiteSpace(payload) ? "{}" : payload,
            Status = AIProposalStatus.Pending,
            ProposedByModel = proposedByModel,
            ProposedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Approve(string? updatedBy)
    {
        if (Status != AIProposalStatus.Pending)
            throw new InvalidOperationException("Solo se pueden aprobar propuestas en estado Pendiente.");
        Status = AIProposalStatus.Approved;
        SetUpdated(updatedBy);
    }

    public void Reject(string reason, string? updatedBy)
    {
        if (Status != AIProposalStatus.Pending)
            throw new InvalidOperationException("Solo se pueden rechazar propuestas en estado Pendiente.");
        if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("El motivo de rechazo es obligatorio.", nameof(reason));

        Status = AIProposalStatus.Rejected;
        RejectionReason = reason.Trim();
        SetUpdated(updatedBy);
    }

    public void MarkExecuted(string? updatedBy)
    {
        if (Status != AIProposalStatus.Approved)
            throw new InvalidOperationException("Solo se pueden ejecutar propuestas aprobadas.");
        Status = AIProposalStatus.Executed;
        SetUpdated(updatedBy);
    }

    public void Cancel(string? updatedBy)
    {
        if (Status is AIProposalStatus.Executed or AIProposalStatus.Cancelled)
            throw new InvalidOperationException("No se puede cancelar una propuesta ya ejecutada o cancelada.");
        Status = AIProposalStatus.Cancelled;
        SetUpdated(updatedBy);
    }
}
