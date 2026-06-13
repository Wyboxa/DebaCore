using Debales.Application.AIGovernance;
using Debales.Application.AIGovernance.Commands.CreateAIRule;
using Debales.Application.Common;
using Debales.Domain.AI;
using NSubstitute;

namespace Debales.Application.Tests.AIGovernance;

public sealed class CreateAIRuleHandlerTests
{
    private readonly IAIRuleRepository _repo = Substitute.For<IAIRuleRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateAIRuleHandler _handler;

    public CreateAIRuleHandlerTests()
    {
        _handler = new CreateAIRuleHandler(_repo, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesRule()
    {
        var command = new CreateAIRuleCommand("Aprobación requerida", "GenerateInvoice", "Toda factura requiere revisión", true, "system");

        var result = await _handler.Handle(command);

        Assert.Equal("Aprobación requerida", result.Name);
        Assert.Equal("GenerateInvoice", result.ActionType);
        Assert.Equal("Toda factura requiere revisión", result.Description);
        Assert.True(result.RequiresApproval);
        Assert.True(result.IsActive);
        await _repo.Received(1).AddAsync(Arg.Any<AIRule>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNoApprovalRequired_SetsFlag()
    {
        var command = new CreateAIRuleCommand("Regla lectura", "ReadData", null, false, "system");

        var result = await _handler.Handle(command);

        Assert.False(result.RequiresApproval);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task Handle_WithEmptyName_Throws()
    {
        var command = new CreateAIRuleCommand("", "GenerateInvoice", null, false, "system");

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command));
        await _repo.DidNotReceive().AddAsync(Arg.Any<AIRule>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_MapsAllDtoFields()
    {
        var command = new CreateAIRuleCommand("Regla test", "TestAction", "Descripción test", true, "admin");

        var result = await _handler.Handle(command);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Regla test", result.Name);
        Assert.Equal("TestAction", result.ActionType);
        Assert.Equal("Descripción test", result.Description);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
    }
}
