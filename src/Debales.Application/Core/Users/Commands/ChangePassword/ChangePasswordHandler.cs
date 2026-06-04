using Debales.Application.Common;

namespace Debales.Application.Core.Users.Commands.ChangePassword;

public sealed class ChangePasswordHandler
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IUnitOfWork _uow;

    public ChangePasswordHandler(IUserRepository users, IPasswordHasher hasher, IUnitOfWork uow)
    {
        _users = users;
        _hasher = hasher;
        _uow = uow;
    }

    public async Task Handle(ChangePasswordCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.NewPassword) || command.NewPassword.Length < 8)
            throw new InvalidOperationException("La contraseña debe tener al menos 8 caracteres.");

        var user = await _users.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        var newHash = _hasher.Hash(command.NewPassword);
        user.UpdatePasswordHash(newHash, command.UpdatedBy);
        _users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
