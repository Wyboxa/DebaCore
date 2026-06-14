using Debales.Domain.AI;

namespace Debales.Domain.Tests.AI;

public sealed class AIRuleTests
{
    [Fact]
    public void Create_WithValidData_Succeeds()
    {
        var rule = AIRule.Create("Requiere aprobación humana", "GenerateInvoice", "Toda factura IA debe aprobarse", true, "system");

        Assert.Equal("Requiere aprobación humana", rule.Name);
        Assert.Equal("GenerateInvoice", rule.ActionType);
        Assert.Equal("Toda factura IA debe aprobarse", rule.Description);
        Assert.True(rule.RequiresApproval);
        Assert.True(rule.IsActive);
        Assert.NotEqual(Guid.Empty, rule.Id);
    }

    [Fact]
    public void Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            AIRule.Create("", "GenerateInvoice", null, false, "system"));
    }

    [Fact]
    public void Create_WithEmptyActionType_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            AIRule.Create("Nombre", "", null, false, "system"));
    }

    [Fact]
    public void Create_WithoutApprovalRequired_IsCorrect()
    {
        var rule = AIRule.Create("Regla sin aprobación", "ReadData", null, false, "system");

        Assert.False(rule.RequiresApproval);
        Assert.True(rule.IsActive);
    }

    [Fact]
    public void Update_ChangesFields()
    {
        var rule = AIRule.Create("Nombre original", "ActionType", null, true, "system");

        rule.Update("Nombre actualizado", "NewActionType", "Descripción nueva", false, "admin");

        Assert.Equal("Nombre actualizado", rule.Name);
        Assert.Equal("NewActionType", rule.ActionType);
        Assert.Equal("Descripción nueva", rule.Description);
        Assert.False(rule.RequiresApproval);
        Assert.NotNull(rule.UpdatedAt);
        Assert.Equal("admin", rule.UpdatedBy);
    }

    [Fact]
    public void Update_WithEmptyName_Throws()
    {
        var rule = AIRule.Create("Nombre", "ActionType", null, false, "system");

        Assert.Throws<ArgumentException>(() => rule.Update("", "ActionType", null, false, "admin"));
    }

    [Fact]
    public void Deactivate_SetsIsActiveFalse()
    {
        var rule = AIRule.Create("Regla", "ActionType", null, false, "system");

        rule.Deactivate("admin");

        Assert.False(rule.IsActive);
        Assert.NotNull(rule.UpdatedAt);
    }
}
