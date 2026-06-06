using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Application.Purchasing.Commands.CreateSupplierPayment;
using Debales.Domain.Purchasing;
using NSubstitute;

namespace Debales.Application.Tests.Purchasing;

public sealed class CreateSupplierPaymentHandlerTests
{
    private readonly ISupplierPaymentRepository _payments = Substitute.For<ISupplierPaymentRepository>();
    private readonly IPayableRepository _payables = Substitute.For<IPayableRepository>();
    private readonly IAccountingEntryService _accounting = Substitute.For<IAccountingEntryService>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreateSupplierPaymentHandler _handler;

    public CreateSupplierPaymentHandlerTests()
    {
        _payments.GetNextNumberAsync(Arg.Any<CancellationToken>()).Returns("PAG-2026-0001");
        _handler = new CreateSupplierPaymentHandler(_payments, _payables, _accounting, _uow);
    }

    private static SupplierPayment BuildSavedPayment(Guid supplierId, decimal amount)
    {
        var payment = SupplierPayment.Create(
            "PAG-2026-0001", supplierId, null,
            DateOnly.FromDateTime(DateTime.Today), amount, null, null, "system");
        return payment;
    }

    [Fact]
    public async Task Handle_WithoutPayable_CreatesPaymentAndCallsAccounting()
    {
        var supplierId = Guid.NewGuid();
        var saved = BuildSavedPayment(supplierId, 750m);
        _payments.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounting.GenerateFromSupplierPaymentAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateOnly>(),
            Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<decimal>(),
            Arg.Any<CancellationToken>()).Returns((Debales.Domain.Accounting.AccountingEntry?)null);

        var command = new CreateSupplierPaymentCommand(supplierId, null, DateOnly.FromDateTime(DateTime.Today), 750m, null, null, "system");
        var result = await _handler.Handle(command);

        Assert.Equal("PAG-2026-0001", result.Number);
        Assert.Equal(750m, result.Amount);
        await _accounting.Received(1).GenerateFromSupplierPaymentAsync(
            Arg.Any<Guid>(), "PAG-2026-0001", Arg.Any<DateOnly>(),
            supplierId, Arg.Any<string?>(), 750m, Arg.Any<CancellationToken>());
        await _uow.Received(2).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithPayable_AppliesPaymentToPayable()
    {
        var supplierId = Guid.NewGuid();
        var payableId = Guid.NewGuid();
        var payable = Payable.Create("VTO-001", Guid.NewGuid(), supplierId, DateOnly.FromDateTime(DateTime.Today), 750m, "system");
        var saved = BuildSavedPayment(supplierId, 750m);

        _payables.GetByIdAsync(payableId, Arg.Any<CancellationToken>()).Returns(payable);
        _payments.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounting.GenerateFromSupplierPaymentAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateOnly>(),
            Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<decimal>(),
            Arg.Any<CancellationToken>()).Returns((Debales.Domain.Accounting.AccountingEntry?)null);

        var command = new CreateSupplierPaymentCommand(supplierId, payableId, DateOnly.FromDateTime(DateTime.Today), 750m, null, null, "system");
        await _handler.Handle(command);

        Assert.Equal(PayableStatus.Settled, payable.Status);
    }

    [Fact]
    public async Task Handle_AccountingReturnsNull_DoesNotThrow()
    {
        var supplierId = Guid.NewGuid();
        var saved = BuildSavedPayment(supplierId, 300m);
        _payments.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounting.GenerateFromSupplierPaymentAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateOnly>(),
            Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<decimal>(),
            Arg.Any<CancellationToken>()).Returns((Debales.Domain.Accounting.AccountingEntry?)null);

        var command = new CreateSupplierPaymentCommand(supplierId, null, DateOnly.FromDateTime(DateTime.Today), 300m, null, null, "system");
        var ex = await Record.ExceptionAsync(() => _handler.Handle(command));

        Assert.Null(ex);
    }
}
