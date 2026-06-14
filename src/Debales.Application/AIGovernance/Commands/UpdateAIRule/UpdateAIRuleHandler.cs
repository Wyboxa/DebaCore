using Debales.Application.AIGovernance.DTOs;

namespace Debales.Application.AIGovernance.Commands.UpdateAIRule;

public sealed class UpdateAIRuleHandler
{
    private readonly IAIRuleRepository _repo;

    public UpdateAIRuleHandler(IAIRuleRepository repo) => _repo = repo;

    public async Task<AIRuleDto> Handle(UpdateAIRuleCommand command, CancellationToken ct = default)
    {
        var rule = await _repo.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Regla no encontrada.");

        rule.Update(command.Name, command.ActionType, command.Description, command.RequiresApproval, command.UpdatedBy);
        _repo.Update(rule);
        return CreateAIRuleHandler.ToDto(rule);
    }
}
