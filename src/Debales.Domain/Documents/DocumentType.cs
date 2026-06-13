using Debales.Domain.Common;

namespace Debales.Domain.Documents;

public sealed class DocumentType : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private DocumentType() { }

    public static DocumentType Create(string name, string? description, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));

        return new DocumentType
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            IsActive = true,
            CreatedBy = createdBy
        };
    }

    public void Update(string name, string? description, string updatedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        SetUpdated(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        IsActive = false;
        SetUpdated(updatedBy);
    }
}
