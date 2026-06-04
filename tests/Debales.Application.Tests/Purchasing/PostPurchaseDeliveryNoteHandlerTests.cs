using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Application.Purchasing;
using Debales.Application.Purchasing.Commands.PostPurchaseDeliveryNote;
using Debales.Domain.Inventory;
using Debales.Domain.Purchasing;
using NSubstitute;

namespace Debales.Application.Tests.Purchasing;

public sealed class PostPurchaseDeliveryNoteHandlerTests
{
    private readonly IPurchaseDeliveryNoteRepository _notes = Substitute.For<IPurchaseDeliveryNoteRepository>();
    private readonly IPurchaseOrderRepository _orders = Substitute.For<IPurchaseOrderRepository>();
    private readonly IStockMovementRepository _movements = Substitute.For<IStockMovementRepository>();
    private readonly IStockBalanceRepository _balances = Substitute.For<IStockBalanceRepository>();
    private readonly IWarehouseRepository _warehouses = Substitute.For<IWarehouseRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly PostPurchaseDeliveryNoteHandler _handler;

    public PostPurchaseDeliveryNoteHandlerTests()
    {
        _movements.GetNextNumberAsync(Arg.Any<CancellationToken>()).Returns("MOV-001");
        _handler = new PostPurchaseDeliveryNoteHandler(_notes, _orders, _movements, _balances, _warehouses, _uow);
    }

    private static PurchaseDeliveryNote BuildDraftNote(Guid? purchaseOrderId = null)
    {
        var note = PurchaseDeliveryNote.Create(
            "ALC-001", Guid.NewGuid(), purchaseOrderId,
            DateOnly.FromDateTime(DateTime.Today), null, "system");
        note.AddLine(null, null, Guid.NewGuid(), "ART-001", "Art 1", null, 5m);
        return note;
    }

    [Fact]
    public async Task Handle_WithWarehouse_CreatesStockInMovements()
    {
        var warehouseId = Guid.NewGuid();
        var warehouse = Warehouse.Create("ALM-01", "Principal", null, "system");
        var note = BuildDraftNote();

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);
        _warehouses.GetAllActiveAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Warehouse> { warehouse }.AsReadOnly() as IReadOnlyList<Warehouse>);
        _balances.GetAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((StockBalance?)null);

        var command = new PostPurchaseDeliveryNoteCommand(note.Id, "web", warehouseId);
        await _handler.Handle(command);

        await _movements.Received(note.Lines.Count)
            .AddAsync(Arg.Is<StockMovement>(m => m.Type == StockMovementType.In), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithLinkedOrder_UpdatesOrderReceiptStatus()
    {
        var orderId = Guid.NewGuid();
        var note = BuildDraftNote(purchaseOrderId: orderId);

        var order = PurchaseOrder.Create(
            "PC-2026-0001", note.SupplierId,
            DateOnly.FromDateTime(DateTime.Today), null, null, "system");
        order.AddLine(Guid.NewGuid(), "ART-001", "Art 1", null, 10m, 5m, 21m);
        order.Confirm("system");

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);
        _orders.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(order);
        _warehouses.GetAllActiveAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Warehouse>().AsReadOnly() as IReadOnlyList<Warehouse>);

        await _handler.Handle(new PostPurchaseDeliveryNoteCommand(note.Id, "web"));

        _orders.Received(1).Update(order);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NoteNotFound_Throws()
    {
        _notes.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((PurchaseDeliveryNote?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new PostPurchaseDeliveryNoteCommand(Guid.NewGuid(), "web")));
    }
}
