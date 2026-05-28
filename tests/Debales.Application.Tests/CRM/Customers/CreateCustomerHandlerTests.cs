using Debales.Application.Common;
using Debales.Application.CRM.Customers;
using Debales.Application.CRM.Customers.Commands.CreateCustomer;
using Debales.Domain.CRM.Customers;
using NSubstitute;

namespace Debales.Application.Tests.CRM.Customers;

public sealed class CreateCustomerHandlerTests
{
    private readonly ICustomerRepository _customers = Substitute.For<ICustomerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _handler = new CreateCustomerHandler(_customers, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesCustomer()
    {
        _customers.ExistsByTaxIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);

        var command = new CreateCustomerCommand("Empresa S.L.", "Tecnología", "B12345678", "+34 91 000 00 00", null, "system");
        var result = await _handler.Handle(command);

        Assert.Equal("Empresa S.L.", result.Name);
        Assert.True(result.IsActive);
        await _customers.Received(1).AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithDuplicateTaxId_Throws()
    {
        _customers.ExistsByTaxIdAsync("B12345678", Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateCustomerCommand("Empresa S.L.", null, "B12345678", null, null, "system");

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command));
    }

    [Fact]
    public async Task Handle_WithoutTaxId_DoesNotCheckDuplicates()
    {
        var command = new CreateCustomerCommand("Empresa S.L.", null, null, null, null, "system");
        var result = await _handler.Handle(command);

        Assert.Equal("Empresa S.L.", result.Name);
        await _customers.DidNotReceive().ExistsByTaxIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
