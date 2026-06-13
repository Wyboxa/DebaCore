namespace Debales.Application.Documents.Commands.DeactivateDocument;

public sealed record DeactivateDocumentCommand(Guid Id, string UpdatedBy);
