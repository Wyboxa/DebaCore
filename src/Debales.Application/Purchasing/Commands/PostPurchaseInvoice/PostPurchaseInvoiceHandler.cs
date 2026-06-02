using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.PostPurchaseInvoice;

public sealed class PostPurchaseInvoiceHandler
{
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IPayableRepository _payables;
    private readonly IAccountingEntryService _accounting;
    private readonly IUnitOfWork _uow;

    public PostPurchaseInvoiceHandler(
        IPurchaseInvoiceRepository invoices, IPayableRepository payables,
        IAccountingEntryService accounting, IUnitOfWork uow)
    {
        _invoices = invoices;
        _payables = payables;
        _accounting = accounting;
        _uow = uow;
    }

    public async Task<PurchaseInvoiceDetailDto> Handle(PostPurchaseInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        var invoice = await _invoices.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Factura '{command.Id}' no encontrada.");

        invoice.Post(command.UpdatedBy);

        var payableNumber = await _payables.GetNextNumberAsync(cancellationToken);
        var payable = Payable.Create(
            payableNumber, invoice.Id, invoice.SupplierId,
            invoice.DueDate, invoice.Total, command.UpdatedBy);

        await _payables.AddAsync(payable, cancellationToken);

        // Asiento contable automático (no bloqueante — si faltan prereqs se omite)
        await _accounting.GenerateFromPurchaseInvoiceAsync(
            invoice.Id, invoice.Number, invoice.Date,
            invoice.SupplierId, invoice.Supplier?.AccountCode,
            invoice.Subtotal, invoice.TaxAmount, invoice.Total,
            cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetPurchaseInvoiceByIdHandler.ToDto(saved!);
    }
}
