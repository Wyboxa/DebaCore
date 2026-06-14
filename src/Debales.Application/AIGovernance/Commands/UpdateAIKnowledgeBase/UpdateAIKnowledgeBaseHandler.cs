using Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;
using Debales.Application.AIGovernance.DTOs;

namespace Debales.Application.AIGovernance.Commands.UpdateAIKnowledgeBase;

public sealed class UpdateAIKnowledgeBaseHandler
{
    private readonly IAIKnowledgeBaseRepository _repo;

    public UpdateAIKnowledgeBaseHandler(IAIKnowledgeBaseRepository repo) => _repo = repo;

    public async Task<AIKnowledgeBaseDto> Handle(UpdateAIKnowledgeBaseCommand command, CancellationToken ct = default)
    {
        var kb = await _repo.GetByIdAsync(command.Id, ct)
            ?? throw new InvalidOperationException("Entrada de conocimiento no encontrada.");

        kb.Update(command.Title, command.Content, command.Category, command.UpdatedBy);
        _repo.Update(kb);
        return CreateAIKnowledgeBaseHandler.ToDto(kb);
    }
}
