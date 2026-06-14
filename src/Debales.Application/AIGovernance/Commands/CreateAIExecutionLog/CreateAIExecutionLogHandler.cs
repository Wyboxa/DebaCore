using Debales.Application.AIGovernance.DTOs;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.CreateAIExecutionLog;

public sealed class CreateAIExecutionLogHandler
{
    private readonly IAIExecutionLogRepository _repo;

    public CreateAIExecutionLogHandler(IAIExecutionLogRepository repo) => _repo = repo;

    public async Task<AIExecutionLogDto> Handle(CreateAIExecutionLogCommand command, CancellationToken ct = default)
    {
        var log = AIExecutionLog.Create(
            command.ActionType, command.ActionDescription, command.ExecutedBy,
            command.Success, command.ResultSummary, command.ErrorMessage, command.AIActionProposalId);

        await _repo.AddAsync(log, ct);
        return ToDto(log);
    }

    internal static AIExecutionLogDto ToDto(AIExecutionLog l) =>
        new(l.Id, l.AIActionProposalId, l.ActionType, l.ActionDescription,
            l.ExecutedBy, l.ExecutedAt, l.Success, l.ResultSummary, l.ErrorMessage);
}
