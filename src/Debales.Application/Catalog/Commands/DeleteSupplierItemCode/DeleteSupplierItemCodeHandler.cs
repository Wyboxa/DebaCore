using Debales.Application.Common;

namespace Debales.Application.Catalog.Commands.DeleteSupplierItemCode;

public sealed class DeleteSupplierItemCodeHandler
{
    private readonly ISupplierItemCodeRepository _codes;
    private readonly IUnitOfWork _uow;

    public DeleteSupplierItemCodeHandler(ISupplierItemCodeRepository codes, IUnitOfWork uow)
    {
        _codes = codes;
        _uow = uow;
    }

    public async Task Handle(DeleteSupplierItemCodeCommand command, CancellationToken cancellationToken = default)
    {
        var code = await _codes.GetBySupplierAndItemAsync(
            command.SupplierId, command.ItemId, cancellationToken)
            ?? throw new KeyNotFoundException("Código de proveedor no encontrado.");

        _codes.Remove(code);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
