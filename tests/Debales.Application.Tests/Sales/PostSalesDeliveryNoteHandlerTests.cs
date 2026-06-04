using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Application.Sales;
using Debales.Application.Sales.Commands.PostSalesDeliveryNote;
using Debales.Domain.Inventory;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Sales;

public sealed class PostSalesDeliveryNoteHandlerTests
{
    private readonly ISalesDeliveryNoteRepository _notes = Substitute.For<ISalesDeliveryNoteRepository>();
    private readonly ISalesOrderRepository _orders = Substitute.For<ISalesOrderRepository>();
    private readonly IStockMovementRepository _movements = Substitute.For<IStockMovementRepository>();
    private readonly IStockBalanceRepository _balances = Substitute.For<IStockBalanceRepository>();
    private readonly IWarehouseRepository _warehouses = Substitute.For<IWarehouseRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly PostSalesDeliveryNoteHandler _handler;

    public PostSalesDeliveryNoteHandlerTests()
    {
        _movements.GetNextNumberAsync(Arg.Any<CancellationToken>()).Returns("MOV-001");
        _handler = new PostSalesDeliveryNoteHandler(_notes, _orders, _movements, _balances, _warehouses, _uow);
    }

    private static SalesDeliveryNote BuildPostedNote(Guid? warehouseId = null)
    {
        var note = SalesDeliveryNote.Create(
            "ALV-001", Guid.NewGuid(), null,
            DateOnly.FromDateTime(DateTime.Today), null, "system");
        note.AddLine(null, null, Guid.NewGuid(), "ART-001", "Art 1", null, 3m);
        return note;
    }

    [Fact]
    public async Task Handle_WithWarehouse_CreatesStockOutMovements()
    {
        var warehouseId = Guid.NewGuid();
        var warehouse = Warehouse.Create("ALM-01", "Principal", null, "system");
        var note = BuildPostedNote();

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);
        _warehouses.GetAllActiveAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Warehouse> { warehouse }.AsReadOnly() as IReadOnlyList<Warehouse>);
        _balances.GetAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((StockBalance?)null);

        var command = new PostSalesDeliveryNoteCommand(note.Id, "web", warehouseId);
        await _handler.Handle(command);

        await _movements.Received(note.Lines.Count)
            .AddAsync(Arg.Is<StockMovement>(m => m.Type == StockMovementType.Out), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNoActiveWarehouse_SkipsStockMovements()
    {
        var note = BuildPostedNote();

        _notes.GetByIdAsync(note.Id, Arg.Any<CancellationToken>()).Returns(note);
        _warehouses.GetAllActiveAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Warehouse>().AsReadOnly() as IReadOnlyList<Warehouse>);

        var command = new PostSalesDeliveryNoteCommand(note.Id, "web");
        await _handler.Handle(command);

        await _movements.DidNotReceive().AddAsync(Arg.Any<StockMovement>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NoteNotFound_Throws()
    {
        _notes.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((SalesDeliveryNote?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new PostSalesDeliveryNoteCommand(Guid.NewGuid(), "web")));
    }
}
