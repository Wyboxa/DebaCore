using Debales.Domain.Common;

namespace Debales.Domain.AI;

public sealed class AIExecutionLog : Entity
{
    public Guid? AIActionProposalId { get; private set; }
    public string ActionType { get; private set; } = default!;
    public string ActionDescription { get; private set; } = default!;
    public string ExecutedBy { get; private set; } = default!;
    public DateTime ExecutedAt { get; private set; }
    public bool Success { get; private set; }
    public string? ResultSummary { get; private set; }
    public string? ErrorMessage { get; private set; }

    private AIExecutionLog() { }

    public static AIExecutionLog Create(
        string actionType,
        string actionDescription,
        string executedBy,
        bool success,
        string? resultSummary = null,
        string? errorMessage = null,
        Guid? proposalId = null)
    {
        if (string.IsNullOrWhiteSpace(actionType)) throw new ArgumentException("El tipo de acción es obligatorio.", nameof(actionType));
        if (string.IsNullOrWhiteSpace(actionDescription)) throw new ArgumentException("La descripción es obligatoria.", nameof(actionDescription));
        if (string.IsNullOrWhiteSpace(executedBy)) throw new ArgumentException("El ejecutor es obligatorio.", nameof(executedBy));

        return new AIExecutionLog
        {
            Id = Guid.NewGuid(),
            AIActionProposalId = proposalId,
            ActionType = actionType.Trim(),
            ActionDescription = actionDescription.Trim(),
            ExecutedBy = executedBy.Trim(),
            ExecutedAt = DateTime.UtcNow,
            Success = success,
            ResultSummary = resultSummary?.Trim(),
            ErrorMessage = errorMessage?.Trim(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = executedBy
        };
    }
}
