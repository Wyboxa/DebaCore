using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.PostSalesInvoice;

public sealed class PostSalesInvoiceHandler
{
    private readonly ISalesInvoiceRepository _invoices;
    private readonly IReceivableRepository _receivables;
    private readonly IAccountingEntryService _accounting;
    private readonly IUnitOfWork _uow;

    public PostSalesInvoiceHandler(
        ISalesInvoiceRepository invoices, IReceivableRepository receivables,
        IAccountingEntryService accounting, IUnitOfWork uow)
    {
        _invoices = invoices;
        _receivables = receivables;
        _accounting = accounting;
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

        // Asiento contable automático (no bloqueante — si faltan prereqs se omite)
        await _accounting.GenerateFromSalesInvoiceAsync(
            invoice.Id, invoice.Number, invoice.Date,
            invoice.CustomerId, invoice.Customer?.AccountCode,
            invoice.Subtotal, invoice.TaxAmount, invoice.Total,
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetSalesInvoiceByIdHandler.ToDto(saved!);
    }
}
