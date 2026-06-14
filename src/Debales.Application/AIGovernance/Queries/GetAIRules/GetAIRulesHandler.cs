using Debales.Application.AIGovernance.Commands.CreateAIRule;
using Debales.Application.AIGovernance.DTOs;

namespace Debales.Application.AIGovernance.Queries.GetAIRules;

public sealed class GetAIRulesHandler
{
    private readonly IAIRuleRepository _repo;

    public GetAIRulesHandler(IAIRuleRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<AIRuleDto>> Handle(CancellationToken ct = default)
    {
        var rules = await _repo.GetAllActiveAsync(ct);
        return rules.Select(CreateAIRuleHandler.ToDto).ToList();
    }
}
