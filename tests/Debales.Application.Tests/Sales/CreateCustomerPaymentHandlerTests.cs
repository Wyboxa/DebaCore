using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Application.Sales.Commands.CreateCustomerPayment;
using Debales.Domain.CRM.Customers;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Sales;

public sealed class CreateCustomerPaymentHandlerTests
{
    private readonly ICustomerPaymentRepository _payments = Substitute.For<ICustomerPaymentRepository>();
    private readonly IReceivableRepository _receivables = Substitute.For<IReceivableRepository>();
    private readonly IAccountingEntryService _accounting = Substitute.For<IAccountingEntryService>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreateCustomerPaymentHandler _handler;

    public CreateCustomerPaymentHandlerTests()
    {
        _payments.GetNextNumberAsync(Arg.Any<CancellationToken>()).Returns("COB-2026-0001");
        _handler = new CreateCustomerPaymentHandler(_payments, _receivables, _accounting, _uow);
    }

    private static CustomerPayment BuildSavedPayment(Guid customerId, decimal amount)
    {
        var payment = CustomerPayment.Create(
            "COB-2026-0001", customerId, null,
            DateOnly.FromDateTime(DateTime.Today), amount, null, null, "system");
        return payment;
    }

    [Fact]
    public async Task Handle_WithoutReceivable_CreatesPaymentAndCallsAccounting()
    {
        var customerId = Guid.NewGuid();
        var saved = BuildSavedPayment(customerId, 500m);
        _payments.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounting.GenerateFromCustomerPaymentAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateOnly>(),
            Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<decimal>(),
            Arg.Any<CancellationToken>()).Returns((Debales.Domain.Accounting.AccountingEntry?)null);

        var command = new CreateCustomerPaymentCommand(customerId, null, DateOnly.FromDateTime(DateTime.Today), 500m, null, null, "system");
        var result = await _handler.Handle(command);

        Assert.Equal("COB-2026-0001", result.Number);
        Assert.Equal(500m, result.Amount);
        await _accounting.Received(1).GenerateFromCustomerPaymentAsync(
            Arg.Any<Guid>(), "COB-2026-0001", Arg.Any<DateOnly>(),
            customerId, Arg.Any<string?>(), 500m, Arg.Any<CancellationToken>());
        await _uow.Received(2).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithReceivable_AppliesPaymentToReceivable()
    {
        var customerId = Guid.NewGuid();
        var receivableId = Guid.NewGuid();
        var receivable = Receivable.Create("VTO-001", Guid.NewGuid(), customerId, DateOnly.FromDateTime(DateTime.Today), 500m, "system");
        var saved = BuildSavedPayment(customerId, 500m);

        _receivables.GetByIdAsync(receivableId, Arg.Any<CancellationToken>()).Returns(receivable);
        _payments.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounting.GenerateFromCustomerPaymentAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateOnly>(),
            Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<decimal>(),
            Arg.Any<CancellationToken>()).Returns((Debales.Domain.Accounting.AccountingEntry?)null);

        var command = new CreateCustomerPaymentCommand(customerId, receivableId, DateOnly.FromDateTime(DateTime.Today), 500m, null, null, "system");
        await _handler.Handle(command);

        // Receivable debe quedar en estado Settled tras pago total
        Assert.Equal(Debales.Domain.Sales.ReceivableStatus.Settled, receivable.Status);
    }

    [Fact]
    public async Task Handle_AccountingReturnsNull_DoesNotThrow()
    {
        var customerId = Guid.NewGuid();
        var saved = BuildSavedPayment(customerId, 200m);
        _payments.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounting.GenerateFromCustomerPaymentAsync(
            Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateOnly>(),
            Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<decimal>(),
            Arg.Any<CancellationToken>()).Returns((Debales.Domain.Accounting.AccountingEntry?)null);

        var command = new CreateCustomerPaymentCommand(customerId, null, DateOnly.FromDateTime(DateTime.Today), 200m, null, null, "system");
        var ex = await Record.ExceptionAsync(() => _handler.Handle(command));

        Assert.Null(ex);
    }
}
