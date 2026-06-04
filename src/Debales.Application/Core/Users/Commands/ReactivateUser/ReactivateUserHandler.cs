using Debales.Application.Common;

namespace Debales.Application.Core.Users.Commands.ReactivateUser;

public sealed class ReactivateUserHandler
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _uow;

    public ReactivateUserHandler(IUserRepository users, IUnitOfWork uow)
    {
        _users = users;
        _uow = uow;
    }

    public async Task Handle(ReactivateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Usuario no encontrado.");

        user.Reactivate(command.UpdatedBy);
        _users.Update(user);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
