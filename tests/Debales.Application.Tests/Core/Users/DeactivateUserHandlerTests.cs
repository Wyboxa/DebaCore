using Debales.Application.Common;
using Debales.Application.Core.Users;
using Debales.Application.Core.Users.Commands.DeactivateUser;
using Debales.Domain.Core.Users;
using NSubstitute;

namespace Debales.Application.Tests.Core.Users;

public sealed class DeactivateUserHandlerTests
{
    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly DeactivateUserHandler _handler;

    private static User MakeActiveUser()
    {
        var email = Email.Create("test@debales.com");
        return User.Create("testuser", email, "hash", "system");
    }

    public DeactivateUserHandlerTests()
    {
        _handler = new DeactivateUserHandler(_users, _uow);
    }

    [Fact]
    public async Task Handle_DeactivatesActiveUser()
    {
        var user = MakeActiveUser();
        Assert.True(user.IsActive);
        _users.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);

        await _handler.Handle(new DeactivateUserCommand(user.Id, "admin"));

        Assert.False(user.IsActive);
        _users.Received(1).Update(user);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithUnknownUser_Throws()
    {
        _users.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(new DeactivateUserCommand(Guid.NewGuid(), "admin")));
    }

    [Fact]
    public async Task Handle_AlreadyInactiveUser_DeactivatesWithoutError()
    {
        var user = MakeActiveUser();
        user.Deactivate("system");
        Assert.False(user.IsActive);
        _users.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);

        await _handler.Handle(new DeactivateUserCommand(user.Id, "admin"));

        Assert.False(user.IsActive);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
