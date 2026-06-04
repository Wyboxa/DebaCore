using Debales.Application.Common;
using Debales.Application.Core.Users;
using Debales.Application.Core.Users.Commands.ChangePassword;
using Debales.Domain.Core.Users;
using NSubstitute;

namespace Debales.Application.Tests.Core.Users;

public sealed class ChangePasswordHandlerTests
{
    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _hasher = Substitute.For<IPasswordHasher>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ChangePasswordHandler _handler;

    public ChangePasswordHandlerTests()
    {
        _hasher.Hash(Arg.Any<string>()).Returns("new_hashed_password");
        _handler = new ChangePasswordHandler(_users, _hasher, _uow);
    }

    [Fact]
    public async Task Handle_WithValidPassword_UpdatesHash()
    {
        var user = User.Create("carlos", Email.Create("carlos@test.com"), "old_hash", "system");
        _users.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);

        await _handler.Handle(new ChangePasswordCommand(user.Id, "NuevaPass123", "admin"));

        _users.Received(1).Update(user);
        await _uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithShortPassword_Throws()
    {
        var userId = Guid.NewGuid();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new ChangePasswordCommand(userId, "short", "admin")));
    }

    [Fact]
    public async Task Handle_UserNotFound_Throws()
    {
        _users.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((User?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(new ChangePasswordCommand(Guid.NewGuid(), "ValidPass123", "admin")));
    }
}
