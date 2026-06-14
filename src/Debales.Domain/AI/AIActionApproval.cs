using Debales.Domain.Common;

namespace Debales.Domain.AI;

public sealed class AIActionApproval : Entity
{
    public Guid AIActionProposalId { get; private set; }
    public AIProposalStatus Decision { get; private set; }
    public string? Notes { get; private set; }
    public string ReviewedBy { get; private set; } = default!;
    public DateTime ReviewedAt { get; private set; }

    private AIActionApproval() { }

    public static AIActionApproval Create(Guid proposalId, AIProposalStatus decision, string reviewedBy, string? notes)
    {
        if (string.IsNullOrWhiteSpace(reviewedBy)) throw new ArgumentException("El revisor es obligatorio.", nameof(reviewedBy));
        if (decision is not (AIProposalStatus.Approved or AIProposalStatus.Rejected))
            throw new ArgumentException("La decisión debe ser Approved o Rejected.", nameof(decision));

        return new AIActionApproval
        {
            Id = Guid.NewGuid(),
            AIActionProposalId = proposalId,
            Decision = decision,
            Notes = notes?.Trim(),
            ReviewedBy = reviewedBy.Trim(),
            ReviewedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = reviewedBy
        };
    }
}
