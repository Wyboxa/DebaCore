using Debales.Application.Common;

namespace Debales.Application.CRM.Contacts.Commands.DeactivateContact;

public sealed class DeactivateContactHandler
{
    private readonly IContactRepository _contacts;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateContactHandler(IContactRepository contacts, IUnitOfWork unitOfWork)
    {
        _contacts = contacts;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeactivateContactCommand command, CancellationToken cancellationToken = default)
    {
        var contact = await _contacts.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Contacto '{command.Id}' no encontrado.");

        contact.Deactivate(command.UpdatedBy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
