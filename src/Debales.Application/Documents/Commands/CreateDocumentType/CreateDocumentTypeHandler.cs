using Debales.Application.Common;
using Debales.Application.Documents.DTOs;
using Debales.Domain.Documents;

namespace Debales.Application.Documents.Commands.CreateDocumentType;

public sealed class CreateDocumentTypeHandler
{
    private readonly IDocumentTypeRepository _types;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDocumentTypeHandler(IDocumentTypeRepository types, IUnitOfWork unitOfWork)
    {
        _types = types;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentTypeDto> Handle(CreateDocumentTypeCommand command, CancellationToken cancellationToken = default)
    {
        var type = DocumentType.Create(command.Name, command.Description, command.CreatedBy);
        await _types.AddAsync(type, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ToDto(type);
    }

    internal static DocumentTypeDto ToDto(DocumentType t) =>
        new(t.Id, t.Name, t.Description, t.IsActive, t.CreatedAt);
}
