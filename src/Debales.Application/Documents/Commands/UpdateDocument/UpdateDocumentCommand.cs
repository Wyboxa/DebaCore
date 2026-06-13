namespace Debales.Application.Documents.Commands.UpdateDocument;

public sealed record UpdateDocumentCommand(
    Guid Id,
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
    string UpdatedBy);
