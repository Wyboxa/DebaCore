using Debales.Application.Accounting;
using Debales.Application.Accounting.Commands.AddRemittanceLine;
using Debales.Application.Accounting.Commands.ConfirmRemittance;
using Debales.Application.Accounting.Commands.CreateRemittance;
using Debales.Application.Accounting.Commands.DeleteRemittance;
using Debales.Application.Accounting.Commands.FailRemittance;
using Debales.Application.Accounting.Commands.RemoveRemittanceLine;
using Debales.Application.Accounting.Commands.SendRemittance;
using Debales.Application.Common;
using Debales.Application.Purchasing;
using Debales.Application.Sales;
using Debales.Domain.Accounting;
using Debales.Domain.Purchasing;
using Debales.Domain.Sales;
using NSubstitute;

namespace Debales.Application.Tests.Accounting;

// ── CreateRemittanceHandlerTests ──────────────────────────────────────────────

public sealed class CreateRemittanceHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IBankAccountRepository _banks = Substitute.For<IBankAccountRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly CreateRemittanceHandler _handler;

    private static readonly Guid _bankId = Guid.NewGuid();
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Today);

    public CreateRemittanceHandlerTests()
    {
        _handler = new CreateRemittanceHandler(_repo, _banks, _uow);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesRemittanceAndReturnsDto()
    {
        var bank = BankAccount.Create("Banco Principal", null, null, null, "EUR", null, "test");
        _banks.GetByIdAsync(_bankId, Arg.Any<CancellationToken>()).Returns(bank);
        _repo.GetNextNumberAsync(RemittanceType.Collection, Arg.Any<CancellationToken>())
             .Returns("REM-C-2026-0001");

        var saved = Remittance.Create("REM-C-2026-0001", _today, _bankId, RemittanceType.Collection, null, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateRemittanceCommand(_today, _bankId, RemittanceType.Collection, null, "test"));

        Assert.Equal("REM-C-2026-0001", result.Number);
        Assert.Equal(RemittanceType.Collection, result.Type);
        Assert.Equal(RemittanceStatus.Draft, result.Status);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_BankAccountNotFound_Throws()
    {
        _banks.GetByIdAsync(_bankId, Arg.Any<CancellationToken>()).Returns((BankAccount?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new CreateRemittanceCommand(_today, _bankId, RemittanceType.Collection, null, "test")));
    }

    [Fact]
    public async Task Handle_GeneratedNumberIsUsed()
    {
        var bank = BankAccount.Create("Banco", null, null, null, "EUR", null, "test");
        _banks.GetByIdAsync(_bankId, Arg.Any<CancellationToken>()).Returns(bank);
        _repo.GetNextNumberAsync(RemittanceType.Payment, Arg.Any<CancellationToken>())
             .Returns("REM-P-2026-0003");

        var saved = Remittance.Create("REM-P-2026-0003", _today, _bankId, RemittanceType.Payment, null, "test");
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(saved);

        var result = await _handler.Handle(
            new CreateRemittanceCommand(_today, _bankId, RemittanceType.Payment, null, "test"));

        Assert.Equal("REM-P-2026-0003", result.Number);
        Assert.Equal(RemittanceType.Payment, result.Type);
    }
}

// ── AddRemittanceLineHandlerTests ────────────────────────────────────────────

public sealed class AddRemittanceLineHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly AddRemittanceLineHandler _handler;

    public AddRemittanceLineHandlerTests()
    {
        _handler = new AddRemittanceLineHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ValidLine_AddsLineAndSaves()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        var docId = Guid.NewGuid();
        var command = new AddRemittanceLineCommand(remittance.Id, docId, 350m, "test");

        Remittance? captured = null;
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
             .Returns(ci =>
             {
                 captured = remittance;
                 return remittance;
             });

        var result = await _handler.Handle(command);

        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.Single(remittance.Lines);
        Assert.Equal(350m, remittance.Lines[0].Amount);
    }

    [Fact]
    public async Task Handle_RemittanceNotFound_Throws()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Remittance?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new AddRemittanceLineCommand(Guid.NewGuid(), Guid.NewGuid(), 100m, "test")));
    }
}

// ── RemoveRemittanceLineHandlerTests ─────────────────────────────────────────

public sealed class RemoveRemittanceLineHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly RemoveRemittanceLineHandler _handler;

    public RemoveRemittanceLineHandlerTests()
    {
        _handler = new RemoveRemittanceLineHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_ExistingLine_RemovesIt()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");

        var docId = Guid.NewGuid();
        remittance.AddLine(docId, 100m, "test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        await _handler.Handle(new RemoveRemittanceLineCommand(remittance.Id, docId, "test"));

        Assert.Empty(remittance.Lines);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RemittanceNotFound_Throws()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Remittance?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new RemoveRemittanceLineCommand(Guid.NewGuid(), Guid.NewGuid(), "test")));
    }
}

// ── SendRemittanceHandlerTests ────────────────────────────────────────────────

public sealed class SendRemittanceHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly SendRemittanceHandler _handler;

    public SendRemittanceHandlerTests()
    {
        _handler = new SendRemittanceHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_DraftWithLines_TransitionsToSent()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");
        remittance.AddLine(Guid.NewGuid(), 100m, "test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        await _handler.Handle(new SendRemittanceCommand(remittance.Id, "test"));

        Assert.Equal(RemittanceStatus.Sent, remittance.Status);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RemittanceNotFound_Throws()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Remittance?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new SendRemittanceCommand(Guid.NewGuid(), "test")));
    }

    [Fact]
    public async Task Handle_DraftWithNoLines_DomainThrows()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new SendRemittanceCommand(remittance.Id, "test")));
    }
}

// ── ConfirmRemittanceHandlerTests ─────────────────────────────────────────────

public sealed class ConfirmRemittanceHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IReceivableRepository _receivables = Substitute.For<IReceivableRepository>();
    private readonly IPayableRepository _payables = Substitute.For<IPayableRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ConfirmRemittanceHandler _handler;

    public ConfirmRemittanceHandlerTests()
    {
        _handler = new ConfirmRemittanceHandler(_repo, _receivables, _payables, _uow);
    }

    [Fact]
    public async Task Handle_CollectionType_AppliesPaymentToReceivables()
    {
        var receivable = Receivable.Create("COB-001", Guid.NewGuid(), Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Today), 500m, "test");

        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");
        remittance.AddLine(receivable.Id, 500m, "test");
        remittance.Send("test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);
        _receivables.GetByIdAsync(receivable.Id, Arg.Any<CancellationToken>()).Returns(receivable);

        await _handler.Handle(new ConfirmRemittanceCommand(remittance.Id, "test"));

        Assert.Equal(RemittanceStatus.Confirmed, remittance.Status);
        Assert.Equal(ReceivableStatus.Settled, receivable.Status);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_PaymentType_AppliesPaymentToPayables()
    {
        var payable = Payable.Create("PAG-001", Guid.NewGuid(), Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Today), 300m, "test");

        var remittance = Remittance.Create("REM-P-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Payment, null, "test");
        remittance.AddLine(payable.Id, 300m, "test");
        remittance.Send("test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);
        _payables.GetByIdAsync(payable.Id, Arg.Any<CancellationToken>()).Returns(payable);

        await _handler.Handle(new ConfirmRemittanceCommand(remittance.Id, "test"));

        Assert.Equal(RemittanceStatus.Confirmed, remittance.Status);
        Assert.Equal(PayableStatus.Settled, payable.Status);
    }

    [Fact]
    public async Task Handle_RemittanceNotFound_Throws()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Remittance?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new ConfirmRemittanceCommand(Guid.NewGuid(), "test")));
    }

    [Fact]
    public async Task Handle_DocumentNotFound_StillConfirms()
    {
        var docId = Guid.NewGuid();
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");
        remittance.AddLine(docId, 100m, "test");
        remittance.Send("test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);
        _receivables.GetByIdAsync(docId, Arg.Any<CancellationToken>()).Returns((Receivable?)null);

        // El handler sigue confirmando la remesa aunque el receivable no se encuentre
        await _handler.Handle(new ConfirmRemittanceCommand(remittance.Id, "test"));

        Assert.Equal(RemittanceStatus.Confirmed, remittance.Status);
    }
}

// ── FailRemittanceHandlerTests ────────────────────────────────────────────────

public sealed class FailRemittanceHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly FailRemittanceHandler _handler;

    public FailRemittanceHandlerTests()
    {
        _handler = new FailRemittanceHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_FromSent_TransitionsToFailed()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");
        remittance.AddLine(Guid.NewGuid(), 100m, "test");
        remittance.Send("test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        await _handler.Handle(new FailRemittanceCommand(remittance.Id, "Rechazo bancario", "test"));

        Assert.Equal(RemittanceStatus.Failed, remittance.Status);
        Assert.Equal("Rechazo bancario", remittance.FailReason);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RemittanceNotFound_Throws()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Remittance?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new FailRemittanceCommand(Guid.NewGuid(), "motivo", "test")));
    }
}

// ── DeleteRemittanceHandlerTests ──────────────────────────────────────────────

public sealed class DeleteRemittanceHandlerTests
{
    private readonly IRemittanceRepository _repo = Substitute.For<IRemittanceRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly DeleteRemittanceHandler _handler;

    public DeleteRemittanceHandlerTests()
    {
        _handler = new DeleteRemittanceHandler(_repo, _uow);
    }

    [Fact]
    public async Task Handle_DraftRemittance_SoftDeletes()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        await _handler.Handle(new DeleteRemittanceCommand(remittance.Id, "test"));

        Assert.True(remittance.IsDeleted);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ConfirmedRemittance_Throws()
    {
        var remittance = Remittance.Create("REM-C-2026-0001",
            DateOnly.FromDateTime(DateTime.Today), Guid.NewGuid(),
            RemittanceType.Collection, null, "test");
        remittance.AddLine(Guid.NewGuid(), 100m, "test");
        remittance.Send("test");
        remittance.Confirm("test");

        _repo.GetByIdAsync(remittance.Id, Arg.Any<CancellationToken>()).Returns(remittance);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new DeleteRemittanceCommand(remittance.Id, "test")));

        await _uow.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_RemittanceNotFound_Throws()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Remittance?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new DeleteRemittanceCommand(Guid.NewGuid(), "test")));
    }
}
