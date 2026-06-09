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
    private readonly IPaymentTermRepository _paymentTerms;
    private readonly IAccountingEntryService _accounting;
    private readonly IUnitOfWork _uow;

    public PostSalesInvoiceHandler(
        ISalesInvoiceRepository invoices, IReceivableRepository receivables,
        IPaymentTermRepository paymentTerms,
        IAccountingEntryService accounting, IUnitOfWork uow)
    {
        _invoices = invoices;
        _receivables = receivables;
        _paymentTerms = paymentTerms;
        _accounting = accounting;
        _uow = uow;
    }

    public async Task<SalesInvoiceDetailDto> Handle(PostSalesInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Factura '{command.Id}' no encontrada.");

        invoice.Post(command.UpdatedBy);

        // Calcular vencimientos según condición de pago del cliente, o crear uno único
        var paymentTermId = invoice.Customer?.PaymentTermId;
        PaymentTerm? paymentTerm = paymentTermId.HasValue
            ? await _paymentTerms.GetByIdAsync(paymentTermId.Value, cancellationToken)
            : null;

        if (paymentTerm is not null)
        {
            var installments = paymentTerm.Calculate(invoice.Date, invoice.Total);
            foreach (var (dueDate, amount) in installments)
            {
                var number = await _receivables.GetNextNumberAsync(cancellationToken);
                var receivable = Receivable.Create(number, invoice.Id, invoice.CustomerId, dueDate, amount, command.UpdatedBy);
                await _receivables.AddAsync(receivable, cancellationToken);
            }
        }
        else
        {
            var receivableNumber = await _receivables.GetNextNumberAsync(cancellationToken);
            var receivable = Receivable.Create(
                receivableNumber, invoice.Id, invoice.CustomerId,
                invoice.DueDate, invoice.Total, command.UpdatedBy);
            await _receivables.AddAsync(receivable, cancellationToken);
        }

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
