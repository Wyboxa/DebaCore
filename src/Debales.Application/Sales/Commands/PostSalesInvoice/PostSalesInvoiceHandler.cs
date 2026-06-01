using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.PostSalesInvoice;

public sealed class PostSalesInvoiceHandler
{
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IReceivableRepository _receivables;
    private readonly IUnitOfWork _uow;

    public PostSalesInvoiceHandler(ISalesInvoiceRepository invoices, IReceivableRepository receivables, IUnitOfWork uow)
    {
        _invoices = invoices;
        _receivables = receivables;
        _uow = uow;
    }

    public async Task<SalesInvoiceDetailDto> Handle(PostSalesInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Factura '{command.Id}' no encontrada.");

        invoice.Post(command.UpdatedBy);

        var receivableNumber = await _receivables.GetNextNumberAsync(cancellationToken);
        var receivable = Receivable.Create(
            receivableNumber,
            invoice.Id,
            invoice.CustomerId,
            invoice.DueDate,
            invoice.Total,
            command.UpdatedBy);

        await _receivables.AddAsync(receivable, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetSalesInvoiceByIdHandler.ToDto(saved!);
    }
}
