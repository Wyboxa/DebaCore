namespace Debales.Application.Documents.Commands.UploadDocumentFile;

public sealed record UploadDocumentFileCommand(
    Guid DocumentId,
    string FileName,
    long FileSizeBytes,
    string MimeType,
    Stream Content,
    string UpdatedBy);
