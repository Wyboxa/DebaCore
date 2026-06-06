using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Purchasing.DTOs;
using Debales.Domain.Purchasing;

namespace Debales.Application.Purchasing.Commands.CreateSupplierPayment;

public sealed class CreateSupplierPaymentHandler
{
    private readonly ISupplierPaymentRepository _payments;
    private readonly IPayableRepository _payables;
    private readonly IAccountingEntryService _accounting;
    private readonly IUnitOfWork _uow;

    public CreateSupplierPaymentHandler(
        ISupplierPaymentRepository payments, IPayableRepository payables,
        IAccountingEntryService accounting, IUnitOfWork uow)
    {
        _payments = payments;
        _payables = payables;
        _accounting = accounting;
        _uow = uow;
    }

    public async Task<SupplierPaymentDetailDto> Handle(CreateSupplierPaymentCommand command, CancellationToken cancellationToken = default)
    {
        if (command.PayableId.HasValue)
        {
            var payable = await _payables.GetByIdAsync(command.PayableId.Value, cancellationToken)
                ?? throw new InvalidOperationException($"Vencimiento '{command.PayableId}' no encontrado.");

            payable.ApplyPayment(command.Amount, command.CreatedBy);
        }

        var number = await _payments.GetNextNumberAsync(cancellationToken);
        var payment = SupplierPayment.Create(
            number, command.SupplierId, command.PayableId,
            command.Date, command.Amount, command.Reference, command.Notes, command.CreatedBy);

        await _payments.AddAsync(payment, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var saved = await _payments.GetByIdAsync(payment.Id, cancellationToken);

        // Asiento contable automático (no bloqueante — si faltan prereqs se omite)
        await _accounting.GenerateFromSupplierPaymentAsync(
            saved!.Id, saved.Number, saved.Date,
            saved.SupplierId, saved.Supplier?.AccountCode,
            saved.Amount, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return ToDetailDto(saved);
    }

    internal static SupplierPaymentDetailDto ToDetailDto(SupplierPayment p) => new(
        p.Id,
        p.Number,
        p.SupplierId,
        p.Supplier?.Name ?? string.Empty,
        p.PayableId,
        p.Payable?.Number,
        p.Date,
        p.Amount,
        p.Reference,
        p.Notes,
        p.CreatedAt,
        p.CreatedBy);
}
