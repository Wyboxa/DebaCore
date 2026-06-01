using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Application.Purchasing.Queries.GetPurchaseInvoiceById;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.PostPurchaseInvoice;

public sealed class PostPurchaseInvoiceHandler
{
    private readonly IPurchaseInvoiceRepository _invoices;
    private readonly IPayableRepository _payables;
    private readonly IUnitOfWork _uow;

    public PostPurchaseInvoiceHandler(IPurchaseInvoiceRepository invoices, IPayableRepository payables, IUnitOfWork uow)
    {
        _invoices = invoices;
        _payables = payables;
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
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _invoices.GetByIdAsync(invoice.Id, cancellationToken);
        return GetPurchaseInvoiceByIdHandler.ToDto(saved!);
    }
}
