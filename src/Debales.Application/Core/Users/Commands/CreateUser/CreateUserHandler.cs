using Debales.Application.Common;
using Debales.Application.Core.Users.DTOs;
using Debales.Domain.Core.Users;

namespace Debales.Application.Core.Users.Commands.CreateUser;

public sealed class CreateUserHandler
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IUserRepository users, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _users = users;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        if (await _users.ExistsByUsernameAsync(command.Username, cancellationToken))
            throw new InvalidOperationException($"Ya existe un usuario con el nombre '{command.Username}'.");

        if (await _users.ExistsByEmailAsync(command.Email, cancellationToken))
            throw new InvalidOperationException($"Ya existe un usuario con el email '{command.Email}'.");

        var email = Email.Create(command.Email);
        var passwordHash = _passwordHasher.Hash(command.Password);
        var user = User.Create(command.Username, email, passwordHash, command.CreatedBy);

        await _users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserDto(user.Id, user.Username, user.Email.Value, user.IsActive, user.CreatedAt, user.CreatedBy);
    }
}
