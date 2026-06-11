using System.Reflection;
using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Application.Sales.Commands.PostSalesInvoice;
using Debales.Application.Sales.Queries.GetSalesInvoiceById;
using Debales.Domain.CRM.Customers;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Sales;

public sealed class PostSalesInvoiceHandlerTests
{
    private readonly ISalesInvoiceRepository _invoices = Substitute.For<ISalesInvoiceRepository>();
    private readonly IReceivableRepository _receivables = Substitute.For<IReceivableRepository>();
    private readonly IPaymentTermRepository _paymentTerms = Substitute.For<IPaymentTermRepository>();
    private readonly IAccountingEntryService _accounting = Substitute.For<IAccountingEntryService>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly PostSalesInvoiceHandler _handler;

    private static int _numberSeq = 1;

    public PostSalesInvoiceHandlerTests()
    {
        _handler = new PostSalesInvoiceHandler(_invoices, _receivables, _paymentTerms, _accounting, _uow);
        _receivables.GetNextNumberAsync(Arg.Any<CancellationToken>())
            .Returns(_ => $"VTO-{_numberSeq++:D3}");
    }

    private static SalesInvoice BuildInvoice(Customer? customer = null)
    {
        var date = new DateOnly(2026, 6, 1);
        var invoice = SalesInvoice.Create("FV-001", Guid.NewGuid(), null, date, date.AddDays(30), null, "system");
        invoice.AddLine(Guid.NewGuid(), "ART-01", "Artículo 1", null, 1m, 1000m, 21m);

        if (customer is not null)
            typeof(SalesInvoice).GetProperty("Customer")!.SetValue(invoice, customer);

        return invoice;
    }

    private static SalesInvoice BuildPostedInvoice()
    {
        var invoice = BuildInvoice();
        invoice.Post("system");
        return invoice;
    }

    [Fact]
    public async Task Handle_NoPaymentTerm_CreatesSingleReceivable()
    {
        var invoice = BuildInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user"));

        await _receivables.Received(1).AddAsync(Arg.Any<Receivable>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NoPaymentTerm_ReceivableUsesInvoiceDueDate()
    {
        var invoice = BuildInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user"));

        await _receivables.Received(1).AddAsync(
            Arg.Is<Receivable>(r => r.DueDate == invoice.DueDate && r.OriginalAmount == invoice.Total),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithPaymentTerm_TwoLines_CreatesTwoReceivables()
    {
        var paymentTermId = Guid.NewGuid();
        var customer = Customer.Create("Cliente Test", null, null, null, null, "system");
        customer.SetPaymentTerm(paymentTermId, "system");

        var paymentTerm = PaymentTerm.Create("30/60",  null,
            [(30, 50m), (60, 50m)], "system");

        var invoice = BuildInvoice(customer);
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);
        _paymentTerms.GetByIdAsync(paymentTermId, Arg.Any<CancellationToken>()).Returns(paymentTerm);

        await _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user"));

        await _receivables.Received(2).AddAsync(Arg.Any<Receivable>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithPaymentTerm_InstallmentAmountsAreCorrect()
    {
        var paymentTermId = Guid.NewGuid();
        var customer = Customer.Create("Cliente Test", null, null, null, null, "system");
        customer.SetPaymentTerm(paymentTermId, "system");

        // 30% a 30 días, 70% a 60 días — total = 1210€ (1000 + 21% IVA)
        var paymentTerm = PaymentTerm.Create("30/70", null,
            [(30, 30m), (60, 70m)], "system");

        var invoice = BuildInvoice(customer);
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);
        _paymentTerms.GetByIdAsync(paymentTermId, Arg.Any<CancellationToken>()).Returns(paymentTerm);

        var capturedReceivables = new List<Receivable>();
        await _receivables.AddAsync(
            Arg.Do<Receivable>(r => capturedReceivables.Add(r)),
            Arg.Any<CancellationToken>());

        await _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user"));

        Assert.Equal(2, capturedReceivables.Count);
        var total = capturedReceivables.Sum(r => r.OriginalAmount);
        Assert.Equal(invoice.Total, total);

        // Primer vencimiento a 30 días, segundo a 60 días
        var sorted = capturedReceivables.OrderBy(r => r.DueDate).ToList();
        Assert.Equal(invoice.Date.AddDays(30), sorted[0].DueDate);
        Assert.Equal(invoice.Date.AddDays(60), sorted[1].DueDate);
    }

    [Fact]
    public async Task Handle_WithPaymentTerm_SingleLine_CreatesSingleReceivable()
    {
        var paymentTermId = Guid.NewGuid();
        var customer = Customer.Create("Cliente Test", null, null, null, null, "system");
        customer.SetPaymentTerm(paymentTermId, "system");

        var paymentTerm = PaymentTerm.Create("Contado", null, [(0, 100m)], "system");

        var invoice = BuildInvoice(customer);
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);
        _paymentTerms.GetByIdAsync(paymentTermId, Arg.Any<CancellationToken>()).Returns(paymentTerm);

        await _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user"));

        await _receivables.Received(1).AddAsync(Arg.Any<Receivable>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_Throws()
    {
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((SalesInvoice?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new PostSalesInvoiceCommand(Guid.NewGuid(), "user")));
    }

    [Fact]
    public async Task Handle_AlreadyPosted_Throws()
    {
        var invoice = BuildPostedInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user")));
    }

    [Fact]
    public async Task Handle_PostsInvoice_GeneratesAccountingEntry()
    {
        var invoice = BuildInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await _handler.Handle(new PostSalesInvoiceCommand(invoice.Id, "user"));

        await _accounting.Received(1).GenerateFromSalesInvoiceAsync(
            invoice.Id, invoice.Number, invoice.Date,
            invoice.CustomerId, Arg.Any<string?>(),
            invoice.Subtotal, invoice.TaxAmount, invoice.Total,
            Arg.Any<CancellationToken>());
    }
}
