namespace Debales.Application.CRM.Notes.DTOs;

public sealed record NoteDto(Guid Id, Guid CustomerId, string Content, DateTime CreatedAt, string? CreatedBy);
