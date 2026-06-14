using Debales.Domain.AI;

namespace Debales.Domain.Tests.AI;

public sealed class AIActionProposalTests
{
    private static AIActionProposal NewPending() =>
        AIActionProposal.Create("Crear factura", "GenerateInvoice", "{}", "Descripción", "SalesInvoice", null, "claude-sonnet-4-6", "system");

    [Fact]
    public void Create_WithValidData_StartsAsPending()
    {
        var proposal = NewPending();

        Assert.Equal(AIProposalStatus.Pending, proposal.Status);
        Assert.Equal("Crear factura", proposal.Title);
        Assert.Equal("GenerateInvoice", proposal.ActionType);
        Assert.NotEqual(Guid.Empty, proposal.Id);
        Assert.True(proposal.ProposedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_WithEmptyTitle_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            AIActionProposal.Create("", "GenerateInvoice"));
    }

    [Fact]
    public void Create_WithEmptyActionType_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            AIActionProposal.Create("Título", ""));
    }

    [Fact]
    public void Create_WithNullPayload_DefaultsToEmptyJson()
    {
        var proposal = AIActionProposal.Create("Título", "ActionType", null);
        Assert.Equal("{}", proposal.Payload);
    }

    [Fact]
    public void Approve_FromPending_TransitionsToApproved()
    {
        var proposal = NewPending();

        proposal.Approve("admin");

        Assert.Equal(AIProposalStatus.Approved, proposal.Status);
        Assert.Equal("admin", proposal.UpdatedBy);
        Assert.NotNull(proposal.UpdatedAt);
    }

    [Fact]
    public void Approve_FromNonPending_Throws()
    {
        var proposal = NewPending();
        proposal.Approve("admin");

        Assert.Throws<InvalidOperationException>(() => proposal.Approve("admin"));
    }

    [Fact]
    public void Reject_FromPending_TransitionsToRejectedWithReason()
    {
        var proposal = NewPending();

        proposal.Reject("No es necesario", "admin");

        Assert.Equal(AIProposalStatus.Rejected, proposal.Status);
        Assert.Equal("No es necesario", proposal.RejectionReason);
    }

    [Fact]
    public void Reject_WithEmptyReason_Throws()
    {
        var proposal = NewPending();

        Assert.Throws<ArgumentException>(() => proposal.Reject("", "admin"));
    }

    [Fact]
    public void Reject_FromNonPending_Throws()
    {
        var proposal = NewPending();
        proposal.Approve("admin");

        Assert.Throws<InvalidOperationException>(() => proposal.Reject("Motivo", "admin"));
    }

    [Fact]
    public void MarkExecuted_FromApproved_TransitionsToExecuted()
    {
        var proposal = NewPending();
        proposal.Approve("admin");

        proposal.MarkExecuted("system");

        Assert.Equal(AIProposalStatus.Executed, proposal.Status);
    }

    [Fact]
    public void MarkExecuted_FromPending_Throws()
    {
        var proposal = NewPending();

        Assert.Throws<InvalidOperationException>(() => proposal.MarkExecuted("system"));
    }

    [Fact]
    public void Cancel_FromPending_TransitionsToCancelled()
    {
        var proposal = NewPending();

        proposal.Cancel("admin");

        Assert.Equal(AIProposalStatus.Cancelled, proposal.Status);
    }

    [Fact]
    public void Cancel_FromApproved_TransitionsToCancelled()
    {
        var proposal = NewPending();
        proposal.Approve("admin");

        proposal.Cancel("admin");

        Assert.Equal(AIProposalStatus.Cancelled, proposal.Status);
    }

    [Fact]
    public void Cancel_FromExecuted_Throws()
    {
        var proposal = NewPending();
        proposal.Approve("admin");
        proposal.MarkExecuted("system");

        Assert.Throws<InvalidOperationException>(() => proposal.Cancel("admin"));
    }

    [Fact]
    public void Cancel_FromCancelled_Throws()
    {
        var proposal = NewPending();
        proposal.Cancel("admin");

        Assert.Throws<InvalidOperationException>(() => proposal.Cancel("admin"));
    }

    [Fact]
    public void Cancel_FromRejected_TransitionsToCancelled()
    {
        var proposal = NewPending();
        proposal.Reject("Motivo", "admin");

        proposal.Cancel("admin");

        Assert.Equal(AIProposalStatus.Cancelled, proposal.Status);
    }
}
