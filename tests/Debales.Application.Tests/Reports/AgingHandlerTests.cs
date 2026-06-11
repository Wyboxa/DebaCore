using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Application.Purchasing.Queries.GetPayablesAging;
using Debales.Application.Sales;
using Debales.Application.Sales.Queries.GetReceivablesAging;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Reports;

// ── GetReceivablesAgingHandlerTests ───────────────────────────────────────────

public sealed class GetReceivablesAgingHandlerTests
{
    private readonly IReceivableRepository _repo = Substitute.For<IReceivableRepository>();
    private readonly GetReceivablesAgingHandler _handler;

    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

    public GetReceivablesAgingHandlerTests()
    {
        _handler = new GetReceivablesAgingHandler(_repo);
    }

    private static Receivable MakeReceivable(DateOnly dueDate, decimal amount)
    {
        var r = Receivable.Create("VTO-001", Guid.NewGuid(), Guid.NewGuid(), dueDate, amount, "test");
        return r;
    }

    [Fact]
    public async Task Handle_NoReceivables_ReturnsZeroReport()
    {
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>())
             .Returns(new List<Receivable>());

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(0m, result.TotalPending);
        Assert.Equal(0m, result.TotalOverdue);
        Assert.Equal(Today, result.AsOf);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Handle_CurrentReceivable_GoesToCurrentBucket()
    {
        var r = MakeReceivable(Today.AddDays(10), 500m);
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new[] { r });

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(500m, result.TotalPending);
        Assert.Equal(0m, result.TotalOverdue);
        Assert.Equal(1, result.Current.Count);
        Assert.Equal(500m, result.Current.Amount);
        Assert.Equal(0, result.Bucket1_30.Count);
    }

    [Fact]
    public async Task Handle_OverdueBy15Days_GoesToBucket1_30()
    {
        var r = MakeReceivable(Today.AddDays(-15), 300m);
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new[] { r });

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(1, result.Bucket1_30.Count);
        Assert.Equal(300m, result.Bucket1_30.Amount);
        Assert.Equal(300m, result.TotalOverdue);
        Assert.Equal(0, result.Current.Count);
    }

    [Fact]
    public async Task Handle_OverdueBy45Days_GoesToBucket31_60()
    {
        var r = MakeReceivable(Today.AddDays(-45), 200m);
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new[] { r });

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(1, result.Bucket31_60.Count);
        Assert.Equal(200m, result.Bucket31_60.Amount);
    }

    [Fact]
    public async Task Handle_OverdueBy75Days_GoesToBucket61_90()
    {
        var r = MakeReceivable(Today.AddDays(-75), 400m);
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new[] { r });

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(1, result.Bucket61_90.Count);
        Assert.Equal(400m, result.Bucket61_90.Amount);
    }

    [Fact]
    public async Task Handle_OverdueBy120Days_GoesToBucketOver90()
    {
        var r = MakeReceivable(Today.AddDays(-120), 600m);
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new[] { r });

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(1, result.BucketOver90.Count);
        Assert.Equal(600m, result.BucketOver90.Amount);
        Assert.Equal(600m, result.TotalOverdue);
    }

    [Fact]
    public async Task Handle_MultipleItems_SumsCorrectly()
    {
        var items = new[]
        {
            MakeReceivable(Today.AddDays(5),   100m),  // current
            MakeReceivable(Today.AddDays(-10), 200m),  // 1-30
            MakeReceivable(Today.AddDays(-45), 300m),  // 31-60
            MakeReceivable(Today.AddDays(-95), 400m),  // >90
        };
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(items);

        var result = await _handler.Handle(new GetReceivablesAgingQuery());

        Assert.Equal(1000m, result.TotalPending);
        Assert.Equal(900m, result.TotalOverdue);   // 200+300+400
        Assert.Equal(4, result.Items.Count);
        Assert.Equal(1, result.Current.Count);
        Assert.Equal(1, result.Bucket1_30.Count);
        Assert.Equal(1, result.Bucket31_60.Count);
        Assert.Equal(1, result.BucketOver90.Count);
    }

    [Fact]
    public async Task Handle_WithCustomerIdFilter_PassesItToRepository()
    {
        var customerId = Guid.NewGuid();
        _repo.GetForAgingAsync(customerId, Arg.Any<CancellationToken>())
             .Returns(new List<Receivable>());

        await _handler.Handle(new GetReceivablesAgingQuery(customerId));

        await _repo.Received(1).GetForAgingAsync(customerId, Arg.Any<CancellationToken>());
    }
}

// ── GetPayablesAgingHandlerTests ──────────────────────────────────────────────

public sealed class GetPayablesAgingHandlerTests
{
    private readonly IPayableRepository _repo = Substitute.For<IPayableRepository>();
    private readonly GetPayablesAgingHandler _handler;

    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

    public GetPayablesAgingHandlerTests()
    {
        _handler = new GetPayablesAgingHandler(_repo);
    }

    private static Payable MakePayable(DateOnly dueDate, decimal amount)
        => Payable.Create("VTP-001", Guid.NewGuid(), Guid.NewGuid(), dueDate, amount, "test");

    [Fact]
    public async Task Handle_NoPayables_ReturnsZeroReport()
    {
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new List<Payable>());

        var result = await _handler.Handle(new GetPayablesAgingQuery());

        Assert.Equal(0m, result.TotalPending);
        Assert.Equal(0m, result.TotalOverdue);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Handle_OverduePayable_ClassifiesCorrectly()
    {
        var p = MakePayable(Today.AddDays(-20), 750m);
        _repo.GetForAgingAsync(null, Arg.Any<CancellationToken>()).Returns(new[] { p });

        var result = await _handler.Handle(new GetPayablesAgingQuery());

        Assert.Equal(1, result.Bucket1_30.Count);
        Assert.Equal(750m, result.TotalOverdue);
    }

    [Fact]
    public async Task Handle_WithSupplierIdFilter_PassesItToRepository()
    {
        var supplierId = Guid.NewGuid();
        _repo.GetForAgingAsync(supplierId, Arg.Any<CancellationToken>()).Returns(new List<Payable>());

        await _handler.Handle(new GetPayablesAgingQuery(supplierId));

        await _repo.Received(1).GetForAgingAsync(supplierId, Arg.Any<CancellationToken>());
    }
}
