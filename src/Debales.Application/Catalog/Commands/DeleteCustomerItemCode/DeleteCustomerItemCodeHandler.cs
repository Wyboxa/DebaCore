using Debales.Application.Common;

namespace Debales.Application.Catalog.Commands.DeleteCustomerItemCode;

public sealed class DeleteCustomerItemCodeHandler
{
    private readonly ICustomerItemCodeRepository _codes;
    private readonly IUnitOfWork _uow;

    public DeleteCustomerItemCodeHandler(ICustomerItemCodeRepository codes, IUnitOfWork uow)
    {
        _codes = codes;
        _uow = uow;
    }

    public async Task Handle(DeleteCustomerItemCodeCommand command, CancellationToken cancellationToken = default)
    {
        var code = await _codes.GetByCustomerAndItemAsync(
            command.CustomerId, command.ItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Código de cliente no encontrado.");

        _codes.Remove(code);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
