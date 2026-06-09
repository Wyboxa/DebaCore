using Debales.Application.Common;
using Debales.Application.Sales;
using Debales.Application.Sales.Commands.CreatePaymentMethod;
using Debales.Application.Sales.Commands.DeletePaymentMethod;
using Debales.Application.Sales.Commands.UpdatePaymentMethod;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Sales;

public sealed class CreatePaymentMethodHandlerTests
{
    private readonly IPaymentMethodRepository _repo = Substitute.For<IPaymentMethodRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreatePaymentMethodHandler _handler;

    public CreatePaymentMethodHandlerTests()
    {
        _handler = new CreatePaymentMethodHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesAndReturnsDto()
    {
        var saved = PaymentMethod.Create("Transferencia", "WIRE", "Transferencia bancaria", "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(new CreatePaymentMethodCommand("Transferencia", "WIRE", "Transferencia bancaria", "test"));

        Assert.Equal("Transferencia", result.Name);
        Assert.Equal("WIRE", result.Code);
        Assert.True(result.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptyName_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(new CreatePaymentMethodCommand("", null, null, "test")));
    }

    [Fact]
    public async Task Handle_CodeIsUppercased()
    {
        var saved = PaymentMethod.Create("Contado", "cash", null, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(new CreatePaymentMethodCommand("Contado", "cash", null, "test"));

        Assert.Equal("CASH", result.Code);
    }
}

public sealed class UpdatePaymentMethodHandlerTests
{
    private readonly IPaymentMethodRepository _repo = Substitute.For<IPaymentMethodRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly UpdatePaymentMethodHandler _handler;

    public UpdatePaymentMethodHandlerTests()
    {
        _handler = new UpdatePaymentMethodHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ExistingMethod_UpdatesSuccessfully()
    {
        var pm = PaymentMethod.Create("Contado", "CASH", null, "system");
        _repo.GetByIdAsync(pm.Id, Arg.Any<CancellationToken>()).Returns(pm);

        var result = await _handler.Handle(new UpdatePaymentMethodCommand(pm.Id, "Efectivo", "CASH", "Pago en metálico", false, "user"));

        Assert.Equal("Efectivo", result.Name);
        Assert.False(result.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsKeyNotFound()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((PaymentMethod?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new UpdatePaymentMethodCommand(Guid.NewGuid(), "X", null, null, true, "user")));
    }
}

public sealed class DeletePaymentMethodHandlerTests
{
    private readonly IPaymentMethodRepository _repo = Substitute.For<IPaymentMethodRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly DeletePaymentMethodHandler _handler;

    public DeletePaymentMethodHandlerTests()
    {
        _handler = new DeletePaymentMethodHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ExistingMethod_SoftDeletes()
    {
        var pm = PaymentMethod.Create("Cheque", "CHECK", null, "system");
        _repo.GetByIdAsync(pm.Id, Arg.Any<CancellationToken>()).Returns(pm);

        await _handler.Handle(new DeletePaymentMethodCommand(pm.Id, "user"));

        Assert.True(pm.IsDeleted);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsKeyNotFound()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((PaymentMethod?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new DeletePaymentMethodCommand(Guid.NewGuid(), "user")));
    }
}
