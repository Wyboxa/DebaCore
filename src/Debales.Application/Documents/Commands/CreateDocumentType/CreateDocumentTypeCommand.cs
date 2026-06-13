namespace Debales.Application.Documents.Commands.CreateDocumentType;

public sealed record CreateDocumentTypeCommand(
    string Name,
    string? Description,
    string CreatedBy);
