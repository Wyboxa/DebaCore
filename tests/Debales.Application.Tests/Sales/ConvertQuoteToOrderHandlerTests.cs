using Debales.Application.Common;
using Debales.Application.Core.NumberSeries;
using Debales.Application.Sales;
using Debales.Application.Sales.Commands.ConvertQuoteToOrder;
using Debales.Application.Sales.Queries.GetSalesOrderById;
using Debales.Domain.Core.NumberSeries;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Sales;

public sealed class ConvertQuoteToOrderHandlerTests
{
    private readonly ISalesQuoteRepository _quotes = Substitute.For<ISalesQuoteRepository>();
    private readonly ISalesOrderRepository _orders = Substitute.For<ISalesOrderRepository>();
    private readonly INumberSeriesRepository _series = Substitute.For<INumberSeriesRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ConvertQuoteToOrderHandler _handler;

    private static NumberSeries BuildSeries() => NumberSeries.Create("PV", "Pedidos venta", "PV-", 5, "system");

    public ConvertQuoteToOrderHandlerTests()
    {
        _series.GetByCodeAsync("PV", Arg.Any<CancellationToken>()).Returns(BuildSeries());
        _handler = new ConvertQuoteToOrderHandler(_quotes, _orders, _series, _uow);
    }

    private static SalesQuote BuildAcceptedQuote()
    {
        var customerId = Guid.NewGuid();
        var today = DateOnly.FromDateTime(DateTime.Today);
        var quote = SalesQuote.Create("PRE-001", customerId, today, today.AddDays(30), null, "system");
        quote.AddLine(Guid.NewGuid(), "ART-01", "Artículo 1", null, 2m, 100m, 21m);
        quote.Send("system");
        quote.Accept("system");
        return quote;
    }

    private static SalesOrder BuildSavedOrder(Guid orderId, Guid customerId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return SalesOrder.Create("PV-00001", customerId, today, null, null, "system");
    }

    [Fact]
    public async Task Handle_AcceptedQuote_CreatesOrder()
    {
        var quote = BuildAcceptedQuote();
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);
        _orders.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ci => BuildSavedOrder((Guid)ci[0], quote.CustomerId));

        await _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user"));

        await _orders.Received(1).AddAsync(Arg.Any<SalesOrder>(), Arg.Any<CancellationToken>());
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_AcceptedQuote_CopiesLines()
    {
        var quote = BuildAcceptedQuote();
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);

        SalesOrder? capturedOrder = null;
        await _orders.AddAsync(
            Arg.Do<SalesOrder>(o => capturedOrder = o),
            Arg.Any<CancellationToken>());
        _orders.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ci => BuildSavedOrder((Guid)ci[0], quote.CustomerId));

        await _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user"));

        Assert.NotNull(capturedOrder);
        Assert.Equal(quote.Lines.Count, capturedOrder!.Lines.Count);
        Assert.Equal(quote.Lines[0].Quantity, capturedOrder.Lines[0].Quantity);
        Assert.Equal(quote.Lines[0].UnitPrice, capturedOrder.Lines[0].UnitPrice);
    }

    [Fact]
    public async Task Handle_AcceptedQuote_SetsCustomerId()
    {
        var quote = BuildAcceptedQuote();
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);

        SalesOrder? capturedOrder = null;
        await _orders.AddAsync(
            Arg.Do<SalesOrder>(o => capturedOrder = o),
            Arg.Any<CancellationToken>());
        _orders.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ci => BuildSavedOrder((Guid)ci[0], quote.CustomerId));

        await _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user"));

        Assert.Equal(quote.CustomerId, capturedOrder!.CustomerId);
    }

    [Fact]
    public async Task Handle_AcceptedQuote_MarksQuoteConverted()
    {
        var quote = BuildAcceptedQuote();
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);
        _orders.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(ci => BuildSavedOrder((Guid)ci[0], quote.CustomerId));

        await _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user"));

        Assert.NotNull(quote.ConvertedToOrderId);
        _quotes.Received(1).Update(quote);
    }

    [Fact]
    public async Task Handle_DraftQuote_Throws()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var quote = SalesQuote.Create("PRE-001", Guid.NewGuid(), today, today.AddDays(30), null, "system");
        quote.AddLine(Guid.NewGuid(), "ART-01", "Art", null, 1m, 100m, 21m);
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user")));
    }

    [Fact]
    public async Task Handle_SentQuote_Throws()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var quote = SalesQuote.Create("PRE-001", Guid.NewGuid(), today, today.AddDays(30), null, "system");
        quote.AddLine(Guid.NewGuid(), "ART-01", "Art", null, 1m, 100m, 21m);
        quote.Send("system");
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user")));
    }

    [Fact]
    public async Task Handle_AlreadyConverted_Throws()
    {
        var quote = BuildAcceptedQuote();
        // Simulate already converted by converting once (sets ConvertedToOrderId)
        var orderId = Guid.NewGuid();
        quote.MarkConvertedToOrder(orderId, "system");
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user")));
    }

    [Fact]
    public async Task Handle_QuoteNotFound_Throws()
    {
        _quotes.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((SalesQuote?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConvertQuoteToOrderCommand(Guid.NewGuid(), null, "user")));
    }

    [Fact]
    public async Task Handle_MissingSeries_Throws()
    {
        var quote = BuildAcceptedQuote();
        _quotes.GetByIdAsync(quote.Id, Arg.Any<CancellationToken>()).Returns(quote);
        _series.GetByCodeAsync("PV", Arg.Any<CancellationToken>()).Returns((NumberSeries?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConvertQuoteToOrderCommand(quote.Id, null, "user")));
    }
}
