using Debales.Application.Common;
using Debales.Application.Core.Users;
using Debales.Application.Core.Users.Commands.CreateUser;
using Debales.Domain.Core.Users;
using NSubstitute;

namespace Debales.Application.Tests.Core.Users;

public sealed class CreateUserHandlerTests
{
    private readonly IUserRepository _users = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _hasher = Substitute.For<IPasswordHasher>();
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _hasher.Hash(Arg.Any<string>()).Returns("hashed_password");
        _handler = new CreateUserHandler(_users, _unitOfWork, _hasher);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CreatesUser()
    {
        _users.ExistsByUsernameAsync("carlos", Arg.Any<CancellationToken>()).Returns(false);
        _users.ExistsByEmailAsync("carlos@debales.com", Arg.Any<CancellationToken>()).Returns(false);

        var command = new CreateUserCommand("carlos", "carlos@debales.com", "Password1!", "system");
        var result = await _handler.Handle(command);

        Assert.Equal("carlos", result.Username);
        Assert.Equal("carlos@debales.com", result.Email);
        Assert.True(result.IsActive);
        await _users.Received(1).AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithDuplicateUsername_Throws()
    {
        _users.ExistsByUsernameAsync("carlos", Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateUserCommand("carlos", "carlos@debales.com", "Password1!", "system");

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command));
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_Throws()
    {
        _users.ExistsByUsernameAsync("carlos2", Arg.Any<CancellationToken>()).Returns(false);
        _users.ExistsByEmailAsync("carlos@debales.com", Arg.Any<CancellationToken>()).Returns(true);

        var command = new CreateUserCommand("carlos2", "carlos@debales.com", "Password1!", "system");

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command));
    }
}
