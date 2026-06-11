using Debales.Application.Accounting;
using Debales.Application.Accounting.Commands.CreateCashAccount;
using Debales.Application.Accounting.Commands.DeleteCashAccount;
using Debales.Application.Accounting.Commands.UpdateCashAccount;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using NSubstitute;

namespace Debales.Application.Tests.Accounting;

public sealed class CreateCashAccountHandlerTests
{
    private readonly ICashAccountRepository _repo = Substitute.For<ICashAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreateCashAccountHandler _handler;

    public CreateCashAccountHandlerTests()
    {
        _handler = new CreateCashAccountHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesAndReturnsDto()
    {
        var saved = CashAccount.Create("CAJA-01", "Caja principal", "EUR", null, "test");
        _repo.ExistsByCodeAsync(Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateCashAccountCommand("caja-01", "Caja principal", "eur", null, "test"));

        Assert.Equal("CAJA-01", result.Code);
        Assert.Equal("Caja principal", result.Name);
        Assert.Equal("EUR", result.CurrencyCode);
        Assert.True(result.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DuplicateCode_ThrowsInvalidOperation()
    {
        _repo.ExistsByCodeAsync("CAJA-01", null, Arg.Any<CancellationToken>()).Returns(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new CreateCashAccountCommand("CAJA-01", "Caja", null, null, "test")));
    }

    [Fact]
    public async Task Handle_EmptyCode_Throws()
    {
        _repo.ExistsByCodeAsync(Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(new CreateCashAccountCommand("", "Caja", null, null, "test")));
    }

    [Fact]
    public async Task Handle_EmptyName_Throws()
    {
        _repo.ExistsByCodeAsync(Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(new CreateCashAccountCommand("CAJA-01", "  ", null, null, "test")));
    }

    [Fact]
    public async Task Handle_CodeUppercased()
    {
        var saved = CashAccount.Create("CAJA-EFECTIVO", "Efectivo", "EUR", null, "test");
        _repo.ExistsByCodeAsync(Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateCashAccountCommand("caja-efectivo", "Efectivo", null, null, "test"));

        Assert.Equal("CAJA-EFECTIVO", result.Code);
    }

    [Fact]
    public async Task Handle_NullCurrency_DefaultsToEur()
    {
        var saved = CashAccount.Create("CAJA-X", "Caja X", null, null, "test");
        _repo.ExistsByCodeAsync(Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateCashAccountCommand("CAJA-X", "Caja X", null, null, "test"));

        Assert.Equal("EUR", result.CurrencyCode);
    }
}

public sealed class UpdateCashAccountHandlerTests
{
    private readonly ICashAccountRepository _repo = Substitute.For<ICashAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly UpdateCashAccountHandler _handler;

    public UpdateCashAccountHandlerTests()
    {
        _handler = new UpdateCashAccountHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ExistingAccount_UpdatesSuccessfully()
    {
        var ca = CashAccount.Create("CAJA-01", "Nombre viejo", "EUR", null, "system");
        _repo.GetByIdAsync(ca.Id, Arg.Any<CancellationToken>()).Returns(ca);
        _repo.ExistsByCodeAsync(Arg.Any<string>(), ca.Id, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _handler.Handle(
            new UpdateCashAccountCommand(ca.Id, "CAJA-01", "Nombre nuevo", "USD", null, false, "user"));

        Assert.Equal("Nombre nuevo", result.Name);
        Assert.Equal("USD", result.CurrencyCode);
        Assert.False(result.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsInvalidOperation()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((CashAccount?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new UpdateCashAccountCommand(Guid.NewGuid(), "CAJA-01", "X", null, null, true, "user")));
    }

    [Fact]
    public async Task Handle_DuplicateCode_ThrowsInvalidOperation()
    {
        var ca = CashAccount.Create("CAJA-01", "Original", "EUR", null, "system");
        _repo.GetByIdAsync(ca.Id, Arg.Any<CancellationToken>()).Returns(ca);
        _repo.ExistsByCodeAsync("CAJA-02", ca.Id, Arg.Any<CancellationToken>()).Returns(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new UpdateCashAccountCommand(ca.Id, "CAJA-02", "Otro nombre", null, null, true, "user")));
    }

    [Fact]
    public async Task Handle_SameCode_DoesNotThrowDuplicate()
    {
        var ca = CashAccount.Create("CAJA-01", "Original", "EUR", null, "system");
        _repo.GetByIdAsync(ca.Id, Arg.Any<CancellationToken>()).Returns(ca);
        _repo.ExistsByCodeAsync("CAJA-01", ca.Id, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _handler.Handle(
            new UpdateCashAccountCommand(ca.Id, "CAJA-01", "Nombre actualizado", "EUR", null, true, "user"));

        Assert.Equal("Nombre actualizado", result.Name);
    }
}

public sealed class DeleteCashAccountHandlerTests
{
    private readonly ICashAccountRepository _repo = Substitute.For<ICashAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly DeleteCashAccountHandler _handler;

    public DeleteCashAccountHandlerTests()
    {
        _handler = new DeleteCashAccountHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ExistingAccount_SoftDeletes()
    {
        var ca = CashAccount.Create("CAJA-01", "Caja Prueba", "EUR", null, "system");
        _repo.GetByIdAsync(ca.Id, Arg.Any<CancellationToken>()).Returns(ca);

        await _handler.Handle(new DeleteCashAccountCommand(ca.Id, "user"));

        Assert.True(ca.IsDeleted);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsInvalidOperation()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((CashAccount?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new DeleteCashAccountCommand(Guid.NewGuid(), "user")));
    }
}
