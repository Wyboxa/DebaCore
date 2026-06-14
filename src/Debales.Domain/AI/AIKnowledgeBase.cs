using Debales.Domain.Common;

namespace Debales.Domain.AI;

public sealed class AIKnowledgeBase : AuditableEntity
{
    public string Title { get; private set; } = default!;
    public string? Category { get; private set; }
    public string Content { get; private set; } = default!;
    public bool IsActive { get; private set; }
    public DateTime? LastReviewedAt { get; private set; }

    private AIKnowledgeBase() { }

    public static AIKnowledgeBase Create(string title, string content, string? category, string? createdBy)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El título es obligatorio.", nameof(title));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("El contenido es obligatorio.", nameof(content));

        return new AIKnowledgeBase
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Content = content.Trim(),
            Category = category?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Update(string title, string content, string? category, string? updatedBy)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El título es obligatorio.", nameof(title));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("El contenido es obligatorio.", nameof(content));

        Title = title.Trim();
        Content = content.Trim();
        Category = category?.Trim();
        LastReviewedAt = DateTime.UtcNow;
        SetUpdated(updatedBy);
    }

    public void Deactivate(string? updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
