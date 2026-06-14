using Debales.Application.AIGovernance.DTOs;
using Debales.Domain.AI;

namespace Debales.Application.AIGovernance.Commands.CreateAIKnowledgeBase;

public sealed class CreateAIKnowledgeBaseHandler
{
    private readonly IAIKnowledgeBaseRepository _repo;

    public CreateAIKnowledgeBaseHandler(IAIKnowledgeBaseRepository repo) => _repo = repo;

    public async Task<AIKnowledgeBaseDto> Handle(CreateAIKnowledgeBaseCommand command, CancellationToken ct = default)
    {
        var kb = AIKnowledgeBase.Create(command.Title, command.Content, command.Category, command.CreatedBy);
        await _repo.AddAsync(kb, ct);
        return ToDto(kb);
    }

    internal static AIKnowledgeBaseDto ToDto(AIKnowledgeBase kb) =>
        new(kb.Id, kb.Title, kb.Category, kb.Content, kb.IsActive, kb.LastReviewedAt, kb.CreatedAt);
}
