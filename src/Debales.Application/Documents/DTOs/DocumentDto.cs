namespace Debales.Application.Documents.DTOs;

public sealed record DocumentDto(
    Guid Id,
    string Title,
    string? Description,
    Guid DocumentTypeId,
    string DocumentTypeName,
    Guid? CustomerId,
    string? CustomerName,
    Guid? SupplierId,
    string? SupplierName,
    string? FileName,
    long? FileSizeBytes,
    string? MimeType,
    string? Notes,
    DateTime DocumentDate,
    bool IsActive,
    DateTime CreatedAt);
