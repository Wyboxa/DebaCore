using Debales.Domain.CRM.Customers;

namespace Debales.Domain.Tests.CRM.Customers;

public sealed class CustomerTests
{
    [Fact]
    public void Customer_Create_WithValidData_Succeeds()
    {
        var customer = Customer.Create("Empresa S.L.", "Tecnología", "B12345678", "+34 91 000 00 00", "system");

        Assert.Equal("Empresa S.L.", customer.Name);
        Assert.Equal("Tecnología", customer.Sector);
        Assert.True(customer.IsActive);
        Assert.NotEqual(Guid.Empty, customer.Id);
    }

    [Fact]
    public void Customer_Create_WithEmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create("", null, null, null, "system"));
    }

    [Fact]
    public void Customer_Update_ChangesFields()
    {
        var customer = Customer.Create("Empresa S.L.", null, null, null, "system");

        customer.Update("Empresa Actualizada S.L.", "Retail", "B99999999", null, "https://empresa.com", "admin");

        Assert.Equal("Empresa Actualizada S.L.", customer.Name);
        Assert.Equal("Retail", customer.Sector);
        Assert.Equal("https://empresa.com", customer.Website);
        Assert.NotNull(customer.UpdatedAt);
    }

    [Fact]
    public void Customer_Deactivate_SetsIsActiveFalse()
    {
        var customer = Customer.Create("Empresa S.L.", null, null, null, "system");

        customer.Deactivate("admin");

        Assert.False(customer.IsActive);
    }

    [Fact]
    public void Address_Create_WithValidData_Succeeds()
    {
        var address = Address.Create("Calle Mayor 1", "Madrid", "28001", "España");

        Assert.Equal("Calle Mayor 1", address.Street);
        Assert.Equal("Madrid", address.City);
    }

    [Fact]
    public void Address_Create_WithEmptyCity_Throws()
    {
        Assert.Throws<ArgumentException>(() => Address.Create("Calle Mayor 1", "", "28001", "España"));
    }

    [Fact]
    public void Customer_SetAddress_AssignsAddress()
    {
        var customer = Customer.Create("Empresa S.L.", null, null, null, "system");
        var address = Address.Create("Calle Mayor 1", "Madrid", "28001", "España");

        customer.SetAddress(address, "admin");

        Assert.NotNull(customer.Address);
        Assert.Equal("Madrid", customer.Address.City);
    }
}
