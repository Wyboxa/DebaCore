using Debales.Domain.Core.Users;

namespace Debales.Domain.Tests.Core.Users;

public sealed class UserTests
{
    [Fact]
    public void User_Create_WithValidData_Succeeds()
    {
        var email = Email.Create("carlos@debales.com");
        var user = User.Create("carlos", email, "hash123", "system");

        Assert.Equal("carlos", user.Username);
        Assert.Equal("carlos@debales.com", user.Email.Value);
        Assert.True(user.IsActive);
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public void User_Create_WithEmptyUsername_Throws()
    {
        var email = Email.Create("carlos@debales.com");

        Assert.Throws<ArgumentException>(() => User.Create("", email, "hash123", "system"));
    }

    [Fact]
    public void User_Deactivate_SetsIsActiveFalse()
    {
        var email = Email.Create("carlos@debales.com");
        var user = User.Create("carlos", email, "hash123", "system");

        user.Deactivate("admin");

        Assert.False(user.IsActive);
        Assert.NotNull(user.UpdatedAt);
    }

    [Fact]
    public void Email_Create_NormalizesToLowercase()
    {
        var email = Email.Create("CARLOS@DEBALES.COM");

        Assert.Equal("carlos@debales.com", email.Value);
    }

    [Fact]
    public void Email_Create_WithInvalidFormat_Throws()
    {
        Assert.Throws<ArgumentException>(() => Email.Create("no-es-un-email"));
    }

    [Fact]
    public void Email_Equality_WorksCorrectly()
    {
        var a = Email.Create("carlos@debales.com");
        var b = Email.Create("CARLOS@DEBALES.COM");

        Assert.Equal(a, b);
    }
}
