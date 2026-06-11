using Debales.Application.Accounting;
using Debales.Application.Accounting.Commands.CreateBankAccount;
using Debales.Application.Accounting.Commands.DeleteBankAccount;
using Debales.Application.Accounting.Commands.UpdateBankAccount;
using Debales.Application.Common;
using Debales.Domain.Accounting;
using NSubstitute;

namespace Debales.Application.Tests.Accounting;

public sealed class CreateBankAccountHandlerTests
{
    private readonly IBankAccountRepository _repo = Substitute.For<IBankAccountRepository>();
    private readonly IAccountRepository _accounts = Substitute.For<IAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreateBankAccountHandler _handler;

    public CreateBankAccountHandlerTests()
    {
        _handler = new CreateBankAccountHandler(_repo, _accounts, _uow);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesAndReturnsDto()
    {
        var saved = BankAccount.Create("Cuenta Principal", "Banco X", "ES7621000418401234567891",
            "CAIXESBBXXX", "EUR", null, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateBankAccountCommand("Cuenta Principal", "Banco X", "ES7621000418401234567891",
                "caixesbbxxx", "eur", null, "test"));

        Assert.Equal("Cuenta Principal", result.Name);
        Assert.Equal("CAIXESBBXXX", result.Bic);
        Assert.Equal("EUR", result.CurrencyCode);
        Assert.True(result.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EmptyName_Throws()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(new CreateBankAccountCommand("  ", null, null, null, null, null, "test")));
    }

    [Fact]
    public async Task Handle_IbanNormalized_RemovesSpacesAndUppercases()
    {
        var saved = BankAccount.Create("Test", null, "ES76 2100 0418 4012 3456 7891", null, null, null, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateBankAccountCommand("Test", null, "ES76 2100 0418 4012 3456 7891", null, null, null, "test"));

        Assert.Equal("ES7621000418401234567891", result.Iban);
    }

    [Fact]
    public async Task Handle_NullCurrency_DefaultsToEur()
    {
        var saved = BankAccount.Create("Caja USD", null, null, null, null, null, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateBankAccountCommand("Caja USD", null, null, null, null, null, "test"));

        Assert.Equal("EUR", result.CurrencyCode);
    }

    [Fact]
    public async Task Handle_WithLinkedAccount_FetchesAccountDetails()
    {
        var accountId = Guid.NewGuid();
        var account = Account.Create("572", "Bancos", AccountType.Asset, true, null, "test");
        var saved = BankAccount.Create("Cuenta Vinculada", null, null, null, null, accountId, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);
        _accounts.GetByIdAsync(accountId, Arg.Any<CancellationToken>()).Returns(account);

        var result = await _handler.Handle(
            new CreateBankAccountCommand("Cuenta Vinculada", null, null, null, null, accountId, "test"));

        await _accounts.Received(1).GetByIdAsync(accountId, Arg.Any<CancellationToken>());
        Assert.Equal("572", result.AccountCode);
        Assert.Equal("Bancos", result.AccountName);
    }
}

public sealed class UpdateBankAccountHandlerTests
{
    private readonly IBankAccountRepository _repo = Substitute.For<IBankAccountRepository>();
    private readonly IAccountRepository _accounts = Substitute.For<IAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly UpdateBankAccountHandler _handler;

    public UpdateBankAccountHandlerTests()
    {
        _handler = new UpdateBankAccountHandler(_repo, _accounts, _uow);
    }

    [Fact]
    public async Task Handle_ExistingAccount_UpdatesSuccessfully()
    {
        var ba = BankAccount.Create("Nombre viejo", "Banco Y", null, null, "EUR", null, "system");
        _repo.GetByIdAsync(ba.Id, Arg.Any<CancellationToken>()).Returns(ba);

        var result = await _handler.Handle(
            new UpdateBankAccountCommand(ba.Id, "Nombre nuevo", "Banco Z", null, null, "USD", null, false, "user"));

        Assert.Equal("Nombre nuevo", result.Name);
        Assert.Equal("USD", result.CurrencyCode);
        Assert.False(result.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsKeyNotFound()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((BankAccount?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new UpdateBankAccountCommand(Guid.NewGuid(), "X", null, null, null, null, null, true, "user")));
    }

    [Fact]
    public async Task Handle_EmptyName_Throws()
    {
        var ba = BankAccount.Create("Original", null, null, null, null, null, "system");
        _repo.GetByIdAsync(ba.Id, Arg.Any<CancellationToken>()).Returns(ba);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(new UpdateBankAccountCommand(ba.Id, "", null, null, null, null, null, true, "user")));
    }
}

public sealed class DeleteBankAccountHandlerTests
{
    private readonly IBankAccountRepository _repo = Substitute.For<IBankAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly DeleteBankAccountHandler _handler;

    public DeleteBankAccountHandlerTests()
    {
        _handler = new DeleteBankAccountHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ExistingAccount_SoftDeletes()
    {
        var ba = BankAccount.Create("Cuenta Prueba", null, null, null, null, null, "system");
        _repo.GetByIdAsync(ba.Id, Arg.Any<CancellationToken>()).Returns(ba);

        await _handler.Handle(new DeleteBankAccountCommand(ba.Id, "user"));

        Assert.True(ba.IsDeleted);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_NotFound_ThrowsKeyNotFound()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((BankAccount?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _handler.Handle(new DeleteBankAccountCommand(Guid.NewGuid(), "user")));
    }
}
