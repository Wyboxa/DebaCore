using Debales.Application.Common;
using Debales.Application.Inventory;
using Debales.Application.Inventory.Commands.CloseInventoryCount;
using Debales.Domain.Inventory;
using NSubstitute;

namespace Debales.Application.Tests.Inventory;

public sealed class CloseInventoryCountHandlerTests
{
    private readonly IInventoryCountRepository _counts = Substitute.For<IInventoryCountRepository>();
    private readonly IStockMovementRepository _movements = Substitute.For<IStockMovementRepository>();
    private readonly IStockBalanceRepository _balances = Substitute.For<IStockBalanceRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CloseInventoryCountHandler _handler;

    public CloseInventoryCountHandlerTests()
    {
        _movements.GetNextNumberAsync(Arg.Any<CancellationToken>()).Returns("MOV-2026-0001");
        _handler = new CloseInventoryCountHandler(_counts, _movements, _balances, _uow);
    }

    private static InventoryCount BuildOpenCount(Guid warehouseId)
        => InventoryCount.Create(
            "REC-2026-0001", DateOnly.FromDateTime(DateTime.Today),
            warehouseId, null, "system");

    [Fact]
    public async Task Handle_NoLines_ClosesWithoutMovements()
    {
        var count = BuildOpenCount(Guid.NewGuid());
        _counts.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(count);

        await _handler.Handle(new CloseInventoryCountCommand(count.Id, "system"));

        await _movements.DidNotReceive().AddAsync(Arg.Any<StockMovement>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.Equal(InventoryCountStatus.Closed, count.Status);
    }

    [Fact]
    public async Task Handle_LineWithDifference_CreatesAdjustmentMovement()
    {
        var warehouseId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var count = BuildOpenCount(warehouseId);
        count.AddLine(itemId, "ART-001", "Artículo", 10m, "system");
        count.SetCountedQuantity(itemId, 12m, "system");

        _counts.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(count);
        _balances.GetAsync(itemId, warehouseId, Arg.Any<CancellationToken>()).Returns((StockBalance?)null);

        await _handler.Handle(new CloseInventoryCountCommand(count.Id, "system"));

        await _movements.Received(1).AddAsync(
            Arg.Is<StockMovement>(m => m.Type == StockMovementType.Adjustment && m.Quantity == 2m),
            Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_LineWithNoDifference_NoMovementCreated()
    {
        var warehouseId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var count = BuildOpenCount(warehouseId);
        count.AddLine(itemId, "ART-001", "Artículo", 10m, "system");
        count.SetCountedQuantity(itemId, 10m, "system");

        _counts.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(count);

        await _handler.Handle(new CloseInventoryCountCommand(count.Id, "system"));

        await _movements.DidNotReceive().AddAsync(Arg.Any<StockMovement>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_LineNotCounted_NoMovementForThatLine()
    {
        var warehouseId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var count = BuildOpenCount(warehouseId);
        count.AddLine(itemId, "ART-001", "Artículo", 10m, "system");

        _counts.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(count);

        await _handler.Handle(new CloseInventoryCountCommand(count.Id, "system"));

        await _movements.DidNotReceive().AddAsync(Arg.Any<StockMovement>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ExistingBalance_AppliesDelta()
    {
        var warehouseId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var count = BuildOpenCount(warehouseId);
        count.AddLine(itemId, "ART-001", "Artículo", 10m, "system");
        count.SetCountedQuantity(itemId, 8m, "system"); // delta = -2

        var balance = StockBalance.Create(itemId, warehouseId);
        balance.Apply(10m);

        _counts.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(count);
        _balances.GetAsync(itemId, warehouseId, Arg.Any<CancellationToken>()).Returns(balance);

        await _handler.Handle(new CloseInventoryCountCommand(count.Id, "system"));

        Assert.Equal(8m, balance.Quantity);
    }

    [Fact]
    public async Task Handle_CountNotFound_Throws()
    {
        _counts.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((InventoryCount?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new CloseInventoryCountCommand(Guid.NewGuid(), "system")));
    }
}
