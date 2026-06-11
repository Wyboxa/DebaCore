using Debales.Application.CRM.Customers;
using Debales.Application.Purchasing;
using Debales.Application.Purchasing.Queries.GetSupplierStatement;
using Debales.Application.Sales;
using Debales.Application.Sales.Queries.GetCustomerStatement;
using Debales.Application.Suppliers;
using Debales.Domain.CRM.Customers;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;
using Debales.Domain.Suppliers;
using NSubstitute;

namespace Debales.Application.Tests.Reports;

// ── GetCustomerStatementHandlerTests ─────────────────────────────────────────

public sealed class GetCustomerStatementHandlerTests
{
    private readonly ICustomerRepository _customers = Substitute.For<ICustomerRepository>();
    private readonly ISalesInvoiceRepository _invoices = Substitute.For<ISalesInvoiceRepository>();
    private readonly ISalesCreditNoteRepository _creditNotes = Substitute.For<ISalesCreditNoteRepository>();
    private readonly ICustomerPaymentRepository _payments = Substitute.For<ICustomerPaymentRepository>();
    private readonly GetCustomerStatementHandler _handler;

    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);
    private static readonly Guid CustomerId = Guid.NewGuid();

    public GetCustomerStatementHandlerTests()
    {
        _handler = new GetCustomerStatementHandler(_customers, _invoices, _creditNotes, _payments);

        var customer = Customer.Create("Cliente Test", null, null, null, null, "test");
        _customers.GetByIdAsync(CustomerId, Arg.Any<CancellationToken>()).Returns(customer);
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new List<SalesInvoice>());
        _creditNotes.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                    .Returns(new List<SalesCreditNote>());
        _payments.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new List<CustomerPayment>());
    }

    private static SalesInvoice MakePostedInvoice(DateOnly date, decimal amount)
    {
        var inv = SalesInvoice.Create("FV-001", CustomerId, null, date, date.AddDays(30), null, "test");
        inv.AddLine(Guid.NewGuid(), "ART-001", "Artículo", null, 1m, amount, 0m);
        inv.Post("test");
        return inv;
    }

    private static SalesCreditNote MakePostedCreditNote(DateOnly date, decimal amount)
    {
        var cn = SalesCreditNote.Create("RV-001", CustomerId, Guid.NewGuid(), date, null, "test");
        cn.AddLine(Guid.NewGuid(), "ART-001", "Artículo", null, 1m, amount, 0m);
        cn.Post("test");
        return cn;
    }

    private static CustomerPayment MakePayment(DateOnly date, decimal amount)
        => CustomerPayment.Create("COB-001", CustomerId, null, date, amount, null, null, "test");

    [Fact]
    public async Task Handle_NoMovements_ReturnsEmptyStatement()
    {
        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Equal(0m, result.TotalDebit);
        Assert.Equal(0m, result.TotalCredit);
        Assert.Equal(0m, result.Balance);
        Assert.Empty(result.Lines);
        Assert.Equal(CustomerId, result.ThirdPartyId);
    }

    [Fact]
    public async Task Handle_PostedInvoice_DebitBalance()
    {
        var inv = MakePostedInvoice(Today, 1000m);
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });

        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Equal(1000m, result.TotalDebit);
        Assert.Equal(0m, result.TotalCredit);
        Assert.Equal(1000m, result.Balance);
        Assert.Single(result.Lines);
        Assert.Equal("Factura", result.Lines[0].Type);
        Assert.Equal(1000m, result.Lines[0].Debit);
        Assert.Equal(0m, result.Lines[0].Credit);
        Assert.Equal(1000m, result.Lines[0].Balance);
    }

    [Fact]
    public async Task Handle_DraftInvoice_Excluded()
    {
        var inv = SalesInvoice.Create("FV-001", CustomerId, null, Today, Today.AddDays(30), null, "test");
        inv.AddLine(Guid.NewGuid(), "ART-001", "Art", null, 1m, 500m, 0m);
        // NOT posted — stays Draft
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });

        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Empty(result.Lines);
        Assert.Equal(0m, result.Balance);
    }

    [Fact]
    public async Task Handle_Payment_ReducesBalance()
    {
        var inv = MakePostedInvoice(Today.AddDays(-10), 1000m);
        var payment = MakePayment(Today, 600m);
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });
        _payments.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { payment });

        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Equal(1000m, result.TotalDebit);
        Assert.Equal(600m, result.TotalCredit);
        Assert.Equal(400m, result.Balance);
        Assert.Equal(2, result.Lines.Count);
        // Last line should have running balance = 400
        Assert.Equal(400m, result.Lines[^1].Balance);
    }

    [Fact]
    public async Task Handle_CreditNote_ReducesBalance()
    {
        var inv = MakePostedInvoice(Today.AddDays(-5), 800m);
        var cn = MakePostedCreditNote(Today, 300m);
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });
        _creditNotes.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                    .Returns(new[] { cn });

        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Equal(800m, result.TotalDebit);
        Assert.Equal(300m, result.TotalCredit);
        Assert.Equal(500m, result.Balance);
        Assert.Equal("Rectificativa", result.Lines[^1].Type);
    }

    [Fact]
    public async Task Handle_LinesOrderedByDate()
    {
        var inv1 = MakePostedInvoice(Today.AddDays(-20), 100m);
        var inv2 = MakePostedInvoice(Today.AddDays(-5), 200m);
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv2, inv1 }); // deliberately reversed

        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Equal(Today.AddDays(-20), result.Lines[0].Date);
        Assert.Equal(Today.AddDays(-5), result.Lines[1].Date);
    }

    [Fact]
    public async Task Handle_RunningBalance_AccumulatesCorrectly()
    {
        var inv = MakePostedInvoice(Today.AddDays(-10), 1000m);
        var payment = MakePayment(Today.AddDays(-5), 400m);
        var cn = MakePostedCreditNote(Today, 200m);
        _invoices.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });
        _payments.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { payment });
        _creditNotes.GetByCustomerForStatementAsync(CustomerId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                    .Returns(new[] { cn });

        var result = await _handler.Handle(new GetCustomerStatementQuery(CustomerId));

        Assert.Equal(3, result.Lines.Count);
        Assert.Equal(1000m, result.Lines[0].Balance); // after invoice
        Assert.Equal(600m, result.Lines[1].Balance);  // after payment
        Assert.Equal(400m, result.Lines[2].Balance);  // after credit note
        Assert.Equal(400m, result.Balance);
    }

    [Fact]
    public async Task Handle_CustomerNotFound_ThrowsInvalidOperation()
    {
        _customers.GetByIdAsync(CustomerId, Arg.Any<CancellationToken>())
                  .Returns((Customer?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new GetCustomerStatementQuery(CustomerId)));
    }
}

// ── GetSupplierStatementHandlerTests ─────────────────────────────────────────

public sealed class GetSupplierStatementHandlerTests
{
    private readonly ISupplierRepository _suppliers = Substitute.For<ISupplierRepository>();
    private readonly IPurchaseInvoiceRepository _invoices = Substitute.For<IPurchaseInvoiceRepository>();
    private readonly IPurchaseCreditNoteRepository _creditNotes = Substitute.For<IPurchaseCreditNoteRepository>();
    private readonly ISupplierPaymentRepository _payments = Substitute.For<ISupplierPaymentRepository>();
    private readonly GetSupplierStatementHandler _handler;

    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);
    private static readonly Guid SupplierId = Guid.NewGuid();

    public GetSupplierStatementHandlerTests()
    {
        _handler = new GetSupplierStatementHandler(_suppliers, _invoices, _creditNotes, _payments);

        var supplier = Supplier.Create("Proveedor Test", null, null, null, null, "test");
        _suppliers.GetByIdAsync(SupplierId, Arg.Any<CancellationToken>()).Returns(supplier);
        _invoices.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new List<PurchaseInvoice>());
        _creditNotes.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                    .Returns(new List<PurchaseCreditNote>());
        _payments.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new List<SupplierPayment>());
    }

    private static PurchaseInvoice MakePostedInvoice(DateOnly date, decimal amount)
    {
        var inv = PurchaseInvoice.Create("FC-001", null, SupplierId, null, date, date.AddDays(30), null, "test");
        inv.AddLine(Guid.NewGuid(), "ART-001", "Artículo", null, 1m, amount, 0m);
        inv.Post("test");
        return inv;
    }

    private static PurchaseCreditNote MakePostedCreditNote(DateOnly date, decimal amount)
    {
        var cn = PurchaseCreditNote.Create("RC-001", SupplierId, Guid.NewGuid(), date, null, "test");
        cn.AddLine(Guid.NewGuid(), "ART-001", "Artículo", null, 1m, amount, 0m);
        cn.Post("test");
        return cn;
    }

    private static SupplierPayment MakePayment(DateOnly date, decimal amount)
        => SupplierPayment.Create("PAG-001", SupplierId, null, date, amount, null, null, "test");

    [Fact]
    public async Task Handle_NoMovements_ReturnsEmptyStatement()
    {
        var result = await _handler.Handle(new GetSupplierStatementQuery(SupplierId));

        Assert.Equal(0m, result.TotalDebit);
        Assert.Equal(0m, result.TotalCredit);
        Assert.Equal(0m, result.Balance);
        Assert.Empty(result.Lines);
    }

    [Fact]
    public async Task Handle_PostedInvoice_CreditBalance()
    {
        var inv = MakePostedInvoice(Today, 2000m);
        _invoices.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });

        var result = await _handler.Handle(new GetSupplierStatementQuery(SupplierId));

        Assert.Equal(0m, result.TotalDebit);
        Assert.Equal(2000m, result.TotalCredit);
        Assert.Equal(2000m, result.Balance); // we owe 2000
        Assert.Equal("Factura", result.Lines[0].Type);
    }

    [Fact]
    public async Task Handle_Payment_ReducesBalance()
    {
        var inv = MakePostedInvoice(Today.AddDays(-10), 2000m);
        var payment = MakePayment(Today, 1500m);
        _invoices.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });
        _payments.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { payment });

        var result = await _handler.Handle(new GetSupplierStatementQuery(SupplierId));

        Assert.Equal(1500m, result.TotalDebit);
        Assert.Equal(2000m, result.TotalCredit);
        Assert.Equal(500m, result.Balance);
    }

    [Fact]
    public async Task Handle_CreditNote_ReducesDebt()
    {
        var inv = MakePostedInvoice(Today.AddDays(-5), 1000m);
        var cn = MakePostedCreditNote(Today, 400m);
        _invoices.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                 .Returns(new[] { inv });
        _creditNotes.GetBySupplierForStatementAsync(SupplierId, Arg.Any<DateOnly?>(), Arg.Any<DateOnly?>(), Arg.Any<CancellationToken>())
                    .Returns(new[] { cn });

        var result = await _handler.Handle(new GetSupplierStatementQuery(SupplierId));

        Assert.Equal(600m, result.Balance);
        Assert.Equal("Rectificativa", result.Lines[^1].Type);
    }

    [Fact]
    public async Task Handle_SupplierNotFound_ThrowsInvalidOperation()
    {
        _suppliers.GetByIdAsync(SupplierId, Arg.Any<CancellationToken>())
                  .Returns((Supplier?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new GetSupplierStatementQuery(SupplierId)));
    }
}

// ── GetTreasuryPositionHandlerTests ──────────────────────────────────────────

public sealed class GetTreasuryPositionHandlerTests
{
    private readonly Debales.Application.Accounting.IBankAccountRepository _banks =
        Substitute.For<Debales.Application.Accounting.IBankAccountRepository>();
    private readonly Debales.Application.Accounting.ICashAccountRepository _cash =
        Substitute.For<Debales.Application.Accounting.ICashAccountRepository>();
    private readonly Debales.Application.Accounting.Queries.GetTreasuryPosition.GetTreasuryPositionHandler _handler;

    public GetTreasuryPositionHandlerTests()
    {
        _handler = new Debales.Application.Accounting.Queries.GetTreasuryPosition.GetTreasuryPositionHandler(_banks, _cash);
        _banks.GetAllActiveAsync(Arg.Any<CancellationToken>()).Returns(new List<Debales.Domain.Accounting.BankAccount>());
        _cash.GetAllActiveAsync(Arg.Any<CancellationToken>()).Returns(new List<Debales.Domain.Accounting.CashAccount>());
    }

    [Fact]
    public async Task Handle_NoAccounts_ReturnsZeroPosition()
    {
        var result = await _handler.Handle(new Debales.Application.Accounting.Queries.GetTreasuryPosition.GetTreasuryPositionQuery());

        Assert.Equal(0m, result.TotalBankBalance);
        Assert.Equal(0m, result.TotalCashBalance);
        Assert.Equal(0m, result.TotalBalance);
        Assert.Empty(result.BankAccounts);
        Assert.Empty(result.CashAccounts);
        Assert.Equal(DateOnly.FromDateTime(DateTime.Today), result.AsOf);
    }

    [Fact]
    public async Task Handle_BankAccountsOnly_BalanceIsZero()
    {
        var b1 = Debales.Domain.Accounting.BankAccount.Create("Banco A", null, null, null, "EUR", null, "test");
        var b2 = Debales.Domain.Accounting.BankAccount.Create("Banco B", null, null, null, "EUR", null, "test");
        _banks.GetAllActiveAsync(Arg.Any<CancellationToken>())
              .Returns(new List<Debales.Domain.Accounting.BankAccount> { b1, b2 });

        var result = await _handler.Handle(new Debales.Application.Accounting.Queries.GetTreasuryPosition.GetTreasuryPositionQuery());

        Assert.Equal(2, result.BankAccounts.Count);
        Assert.Equal(0m, result.TotalBankBalance); // BankAccount has no balance field yet
        Assert.Equal(0m, result.TotalBalance);
    }

    [Fact]
    public async Task Handle_CashAccountsWithBalance_SumsCorrectly()
    {
        var c1 = Debales.Domain.Accounting.CashAccount.Create("CAJA-01", "Caja A", null, null, "test");
        var c2 = Debales.Domain.Accounting.CashAccount.Create("CAJA-02", "Caja B", null, null, "test");
        // CashAccount.CurrentBalance is 0 by default; use Deposit to set it
        c1.AdjustBalance(500m, "test");
        c2.AdjustBalance(300m, "test");
        _cash.GetAllActiveAsync(Arg.Any<CancellationToken>())
             .Returns(new List<Debales.Domain.Accounting.CashAccount> { c1, c2 });

        var result = await _handler.Handle(new Debales.Application.Accounting.Queries.GetTreasuryPosition.GetTreasuryPositionQuery());

        Assert.Equal(800m, result.TotalCashBalance);
        Assert.Equal(800m, result.TotalBalance);
        Assert.Equal(2, result.CashAccounts.Count);
    }

    [Fact]
    public async Task Handle_MixedAccounts_TotalIsBankPlusCash()
    {
        var bank = Debales.Domain.Accounting.BankAccount.Create("BCO", null, null, null, "EUR", null, "test");
        var cash = Debales.Domain.Accounting.CashAccount.Create("CAJ-01", "Caja", null, null, "test");
        cash.AdjustBalance(1200m, "test");
        _banks.GetAllActiveAsync(Arg.Any<CancellationToken>())
              .Returns(new List<Debales.Domain.Accounting.BankAccount> { bank });
        _cash.GetAllActiveAsync(Arg.Any<CancellationToken>())
             .Returns(new List<Debales.Domain.Accounting.CashAccount> { cash });

        var result = await _handler.Handle(new Debales.Application.Accounting.Queries.GetTreasuryPosition.GetTreasuryPositionQuery());

        Assert.Equal(0m, result.TotalBankBalance);
        Assert.Equal(1200m, result.TotalCashBalance);
        Assert.Equal(1200m, result.TotalBalance);
    }
}
