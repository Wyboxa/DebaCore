using Debales.Application.Catalog;
using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Application.Purchasing.Commands.GenerateInvoiceFromPurchaseDeliveryNote;
using Debales.Domain.Purchasing;
using NSubstitute;

namespace Debales.Application.Tests.Purchasing;

public sealed class GenerateInvoiceFromPurchaseDeliveryNoteHandlerTests
{
    private readonly IPurchaseDeliveryNoteRepository _notes = Substitute.For<IPurchaseDeliveryNoteRepository>();
    private readonly IPurchaseOrderRepository _orders = Substitute.For<IPurchaseOrderRepository>();
    private readonly IPurchaseInvoiceRepository _invoices = Substitute.For<IPurchaseInvoiceRepository>();
    private readonly IItemRepository _items = Substitute.For<IItemRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly GenerateInvoiceFromPurchaseDeliveryNoteHandler _handler;

    public GenerateInvoiceFromPurchaseDeliveryNoteHandlerTests()
    {
        _invoices.GetNextNumberAsync(Arg.Any<CancellationToken>()).Returns("FC-2026-0001");
        _handler = new GenerateInvoiceFromPurchaseDeliveryNoteHandler(_notes, _orders, _invoices, _items, _uow);
    }

    private static PurchaseDeliveryNote BuildPostedNote()
    {
        var note = PurchaseDeliveryNote.Create(
            "ALC-001", Guid.NewGuid(), null,
            DateOnly.FromDateTime(DateTime.Today), null, "system");
        note.AddLine(null, null, Guid.NewGuid(), "ART-001", "Art 1", null, 2m);
        note.Post("system");
        return note;
    }

    [Fact]
    public async Task Handle_PostedNote_CreatesInvoice()
    {
        var note = BuildPostedNote();
        var dueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

        var savedInvoice = PurchaseInvoice.Create(
            "FC-2026-0001", null, note.SupplierId, note.Id,
            DateOnly.FromDateTime(DateTime.Today), dueDate, null, "web");

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);
        _invoices.GetByPurchaseDeliveryNoteIdAsync(note.Id, Arg.Any<CancellationToken>())
            .Returns((PurchaseInvoice?)null);
        _invoices.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(savedInvoice);

        await _handler.Handle(new GenerateInvoiceFromPurchaseDeliveryNoteCommand(note.Id, dueDate, "web"));

        await _invoices.Received(1).AddAsync(Arg.Any<PurchaseInvoice>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DraftNote_Throws()
    {
        var note = PurchaseDeliveryNote.Create(
            "ALC-002", Guid.NewGuid(), null,
            DateOnly.FromDateTime(DateTime.Today), null, "system");
        var dueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new GenerateInvoiceFromPurchaseDeliveryNoteCommand(note.Id, dueDate, "web")));
    }

    [Fact]
    public async Task Handle_AlreadyInvoiced_Throws()
    {
        var note = BuildPostedNote();
        var existingInvoice = PurchaseInvoice.Create(
            "FC-2026-0001", null, note.SupplierId, note.Id,
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
            null, "system");

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);
        _invoices.GetByPurchaseDeliveryNoteIdAsync(note.Id, Arg.Any<CancellationToken>())
            .Returns(existingInvoice);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new GenerateInvoiceFromPurchaseDeliveryNoteCommand(
                note.Id, DateOnly.FromDateTime(DateTime.Today.AddDays(30)), "web")));
    }
}
