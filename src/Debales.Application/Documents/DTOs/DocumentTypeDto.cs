namespace Debales.Application.Documents.DTOs;

public sealed record DocumentTypeDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAt);
