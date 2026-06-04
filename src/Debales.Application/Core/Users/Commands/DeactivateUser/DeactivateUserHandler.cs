using Debales.Application.Common;

namespace Debales.Application.Core.Users.Commands.DeactivateUser;

public sealed class DeactivateUserHandler
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _uow;

    public DeactivateUserHandler(IUserRepository users, IUnitOfWork uow)
    {
        _users = users;
        _uow = uow;
    }

    public async Task Handle(DeactivateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        user.Deactivate(command.UpdatedBy);
        _users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
