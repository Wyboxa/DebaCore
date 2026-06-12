using Debales.Application.Common;

namespace Debales.Application.Suppliers.Contacts.Commands.DeactivateSupplierContact;

public sealed class DeactivateSupplierContactHandler
{
    private readonly ISupplierContactRepository _contacts;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateSupplierContactHandler(ISupplierContactRepository contacts, IUnitOfWork unitOfWork)
    {
        _contacts = contacts;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeactivateSupplierContactCommand command, CancellationToken cancellationToken = default)
    {
        var contact = await _contacts.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Contacto '{command.Id}' no encontrado.");

        contact.Deactivate(command.UpdatedBy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
