namespace Debales.Application.Documents.Commands.UpdateDocumentType;

public sealed record UpdateDocumentTypeCommand(
    Guid Id,
    string Name,
    string? Description,
    string UpdatedBy);
