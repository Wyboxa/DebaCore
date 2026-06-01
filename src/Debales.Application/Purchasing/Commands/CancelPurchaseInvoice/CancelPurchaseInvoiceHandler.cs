using Debales.Application.Common;

namespace Debales.Application.Purchasing.Commands.CancelPurchaseInvoice;

public sealed class CancelPurchaseInvoiceHandler
{
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IUnitOfWork _uow;

    public CancelPurchaseInvoiceHandler(IPurchaseInvoiceRepository invoices, IUnitOfWork uow)
    {
        _invoices = invoices;
        _uow = uow;
    }

    public async Task Handle(CancelPurchaseInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Factura '{command.Id}' no encontrada.");

        invoice.Cancel(command.UpdatedBy);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
