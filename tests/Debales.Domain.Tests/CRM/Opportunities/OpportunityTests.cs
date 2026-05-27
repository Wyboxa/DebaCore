using Debales.Domain.CRM.Opportunities;

namespace Debales.Domain.Tests.CRM.Opportunities;

public sealed class OpportunityTests
{
    [Fact]
    public void Opportunity_Create_DefaultsToNew()
    {
        var opp = Opportunity.Create(Guid.NewGuid(), "Proyecto CRM", "system", 15000m);

        Assert.Equal(OpportunityStatus.New, opp.Status);
        Assert.Equal(15000m, opp.EstimatedValue);
    }

    [Fact]
    public void Opportunity_UpdateStatus_ChangesStatus()
    {
        var opp = Opportunity.Create(Guid.NewGuid(), "Proyecto CRM", "system");

        opp.UpdateStatus(OpportunityStatus.Won, "admin");

        Assert.Equal(OpportunityStatus.Won, opp.Status);
        Assert.NotNull(opp.UpdatedAt);
    }

    [Fact]
    public void Opportunity_Create_WithEmptyTitle_Throws()
    {
        Assert.Throws<ArgumentException>(() => Opportunity.Create(Guid.NewGuid(), "", "system"));
    }
}
