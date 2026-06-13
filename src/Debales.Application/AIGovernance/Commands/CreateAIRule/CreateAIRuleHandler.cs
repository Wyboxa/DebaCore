using Debales.Application.AIGovernance.DTOs;
using Debales.Application.Common;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.CreateAIRule;

public sealed class CreateAIRuleHandler
{
    private readonly IAIRuleRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAIRuleHandler(IAIRuleRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<AIRuleDto> Handle(CreateAIRuleCommand command, CancellationToken ct = default)
    {
        var rule = AIRule.Create(command.Name, command.ActionType, command.Description, command.RequiresApproval, command.CreatedBy);
        await _repo.AddAsync(rule, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return ToDto(rule);
    }

    internal static AIRuleDto ToDto(AIRule r) =>
        new(r.Id, r.Name, r.Description, r.ActionType, r.RequiresApproval, r.IsActive, r.CreatedAt);
}
