using Debales.Application.Common;

namespace Debales.Application.Documents.Commands.DeactivateDocument;

public sealed class DeactivateDocumentHandler
{
    private readonly IDocumentRepository _documents;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateDocumentHandler(IDocumentRepository documents, IUnitOfWork unitOfWork)
    {
        _documents = documents;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeactivateDocumentCommand command, CancellationToken cancellationToken = default)
    {
        var document = await _documents.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Documento '{command.Id}' no encontrado.");

        document.Deactivate(command.UpdatedBy);
        _documents.Update(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
