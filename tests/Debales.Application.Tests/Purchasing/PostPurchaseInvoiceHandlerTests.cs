using System.Reflection;
using Debales.Application.Accounting.Services;
using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Application.Purchasing.Commands.PostPurchaseInvoice;
using Debales.Application.Sales;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;
using Debales.Domain.Suppliers;
using NSubstitute;

namespace Debales.Application.Tests.Purchasing;

public sealed class PostPurchaseInvoiceHandlerTests
{
    private readonly IPurchaseInvoiceRepository _invoices = Substitute.For<IPurchaseInvoiceRepository>();
    private readonly IPayableRepository _payables = Substitute.For<IPayableRepository>();
    private readonly IPaymentTermRepository _paymentTerms = Substitute.For<IPaymentTermRepository>();
    private readonly IAccountingEntryService _accounting = Substitute.For<IAccountingEntryService>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly PostPurchaseInvoiceHandler _handler;

    private static int _numberSeq = 1;

    public PostPurchaseInvoiceHandlerTests()
    {
        _handler = new PostPurchaseInvoiceHandler(_invoices, _payables, _paymentTerms, _accounting, _uow);
        _payables.GetNextNumberAsync(Arg.Any<CancellationToken>())
            .Returns(_ => $"VPG-{_numberSeq++:D3}");
    }

    private static PurchaseInvoice BuildInvoice(Supplier? supplier = null)
    {
        var date = new DateOnly(2026, 6, 1);
        var invoice = PurchaseInvoice.Create("FC-001", "EXT-001", Guid.NewGuid(), null, date, date.AddDays(30), null, "system");
        invoice.AddLine(Guid.NewGuid(), "ART-01", "Artículo 1", null, 2m, 500m, 21m);

        if (supplier is not null)
            typeof(PurchaseInvoice).GetProperty("Supplier")!.SetValue(invoice, supplier);

        return invoice;
    }

    private static PurchaseInvoice BuildPostedInvoice()
    {
        var invoice = BuildInvoice();
        invoice.Post("system");
        return invoice;
    }

    [Fact]
    public async Task Handle_NoPaymentTerm_CreatesSinglePayable()
    {
        var invoice = BuildInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await _handler.Handle(new PostPurchaseInvoiceCommand(invoice.Id, "user"));

        await _payables.Received(1).AddAsync(Arg.Any<Payable>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NoPaymentTerm_PayableUsesInvoiceDueDate()
    {
        var invoice = BuildInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await _handler.Handle(new PostPurchaseInvoiceCommand(invoice.Id, "user"));

        await _payables.Received(1).AddAsync(
            Arg.Is<Payable>(p => p.DueDate == invoice.DueDate && p.OriginalAmount == invoice.Total),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithPaymentTerm_TwoLines_CreatesTwoPayables()
    {
        var paymentTermId = Guid.NewGuid();
        var supplier = Supplier.Create("Proveedor Test", null, null, null, null, "system");
        supplier.SetPaymentTerm(paymentTermId, "system");

        var paymentTerm = PaymentTerm.Create("30/60", null, [(30, 50m), (60, 50m)], "system");

        var invoice = BuildInvoice(supplier);
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);
        _paymentTerms.GetByIdAsync(paymentTermId, Arg.Any<CancellationToken>()).Returns(paymentTerm);

        await _handler.Handle(new PostPurchaseInvoiceCommand(invoice.Id, "user"));

        await _payables.Received(2).AddAsync(Arg.Any<Payable>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithPaymentTerm_AmountsSumToTotal()
    {
        var paymentTermId = Guid.NewGuid();
        var supplier = Supplier.Create("Proveedor Test", null, null, null, null, "system");
        supplier.SetPaymentTerm(paymentTermId, "system");

        var paymentTerm = PaymentTerm.Create("25/75", null, [(15, 25m), (45, 75m)], "system");

        var invoice = BuildInvoice(supplier);
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);
        _paymentTerms.GetByIdAsync(paymentTermId, Arg.Any<CancellationToken>()).Returns(paymentTerm);

        var captured = new List<Payable>();
        await _payables.AddAsync(
            Arg.Do<Payable>(p => captured.Add(p)),
            Arg.Any<CancellationToken>());

        await _handler.Handle(new PostPurchaseInvoiceCommand(invoice.Id, "user"));

        Assert.Equal(2, captured.Count);
        Assert.Equal(invoice.Total, captured.Sum(p => p.OriginalAmount));
    }

    [Fact]
    public async Task Handle_NotFound_Throws()
    {
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((PurchaseInvoice?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new PostPurchaseInvoiceCommand(Guid.NewGuid(), "user")));
    }

    [Fact]
    public async Task Handle_AlreadyPosted_Throws()
    {
        var invoice = BuildPostedInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new PostPurchaseInvoiceCommand(invoice.Id, "user")));
    }

    [Fact]
    public async Task Handle_PostsInvoice_GeneratesAccountingEntry()
    {
        var invoice = BuildInvoice();
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(invoice);

        await _handler.Handle(new PostPurchaseInvoiceCommand(invoice.Id, "user"));

        await _accounting.Received(1).GenerateFromPurchaseInvoiceAsync(
            invoice.Id, invoice.Number, invoice.Date,
            invoice.SupplierId, Arg.Any<string?>(),
            invoice.Subtotal, invoice.TaxAmount, invoice.Total,
            Arg.Any<CancellationToken>());
    }
}
