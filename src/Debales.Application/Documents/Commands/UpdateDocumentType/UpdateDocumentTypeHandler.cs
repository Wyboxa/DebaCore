using Debales.Application.Common;
using Debales.Application.Documents.Commands.CreateDocumentType;
using Debales.Application.Documents.DTOs;

namespace Debales.Application.Documents.Commands.UpdateDocumentType;

public sealed class UpdateDocumentTypeHandler
{
    private readonly IDocumentTypeRepository _types;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDocumentTypeHandler(IDocumentTypeRepository types, IUnitOfWork unitOfWork)
    {
        _types = types;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentTypeDto> Handle(UpdateDocumentTypeCommand command, CancellationToken cancellationToken = default)
    {
        var type = await _types.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tipo de documento '{command.Id}' no encontrado.");

        type.Update(command.Name, command.Description, command.UpdatedBy);
        _types.Update(type);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return CreateDocumentTypeHandler.ToDto(type);
    }
}
