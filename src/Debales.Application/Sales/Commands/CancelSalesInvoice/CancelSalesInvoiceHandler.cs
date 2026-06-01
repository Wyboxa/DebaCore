using Debales.Application.Common;

namespace Debales.Application.Sales.Commands.CancelSalesInvoice;

public sealed class CancelSalesInvoiceHandler
{
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IUnitOfWork _uow;

    public CancelSalesInvoiceHandler(ISalesInvoiceRepository invoices, IUnitOfWork uow)
    {
        _invoices = invoices;
        _uow = uow;
    }

    public async Task Handle(CancelSalesInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Factura '{command.Id}' no encontrada.");

        invoice.Cancel(command.UpdatedBy);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
