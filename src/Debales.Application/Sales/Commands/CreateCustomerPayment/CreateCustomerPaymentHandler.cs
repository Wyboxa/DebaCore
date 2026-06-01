using Debales.Application.Common;
using Debales.Application.Sales.DTOs;
using Debales.Domain.Sales;

namespace Debales.Application.Sales.Commands.CreateCustomerPayment;

public sealed class CreateCustomerPaymentHandler
{
    private readonly ICustomerPaymentRepository _payments;
    private readonly IReceivableRepository _receivables;
    private readonly IUnitOfWork _uow;

    public CreateCustomerPaymentHandler(ICustomerPaymentRepository payments, IReceivableRepository receivables, IUnitOfWork uow)
    {
        _payments = payments;
        _receivables = receivables;
        _uow = uow;
    }

    public async Task<CustomerPaymentDetailDto> Handle(CreateCustomerPaymentCommand command, CancellationToken cancellationToken = default)
    {
        Receivable? receivable = null;
        if (command.ReceivableId.HasValue)
        {
            receivable = await _receivables.GetByIdAsync(command.ReceivableId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"Vencimiento '{command.ReceivableId}' no encontrado.");

            receivable.ApplyPayment(command.Amount, command.CreatedBy);
        }

        var number = await _payments.GetNextNumberAsync(cancellationToken);
        var payment = CustomerPayment.Create(
            number, command.CustomerId, command.ReceivableId,
            command.Date, command.Amount, command.Reference, command.Notes, command.CreatedBy);

        await _payments.AddAsync(payment, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _payments.GetByIdAsync(payment.Id, cancellationToken);
        return ToDetailDto(saved!);
    }

    internal static CustomerPaymentDetailDto ToDetailDto(CustomerPayment p) => new(
        p.Id,
        p.Number,
        p.CustomerId,
        p.Customer?.Name ?? string.Empty,
        p.ReceivableId,
        p.Receivable?.Number,
        p.Date,
        p.Amount,
        p.Reference,
        p.Notes,
        p.CreatedAt,
        p.CreatedBy);
}
