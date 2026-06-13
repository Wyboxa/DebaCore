namespace Debales.Application.Documents.Commands.CreateDocument;

public sealed record CreateDocumentCommand(
    string Title,
    string? Description,
    Guid DocumentTypeId,
    Guid? CustomerId,
    Guid? SupplierId,
    string? FileName,
    long? FileSizeBytes,
    string? MimeType,
    string? Notes,
    DateTime DocumentDate,
    string CreatedBy);
