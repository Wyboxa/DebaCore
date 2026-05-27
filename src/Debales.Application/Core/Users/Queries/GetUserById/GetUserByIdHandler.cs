using Debales.Application.Core.Users.DTOs;

namespace Debales.Application.Core.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler
{
    private readonly IUserRepository _users;

    public GetUserByIdHandler(IUserRepository users) => _users = users;

    public async Task<UserDto?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(query.UserId, cancellationToken);

        if (user is null)
            return null;

        return new UserDto(user.Id, user.Username, user.Email.Value, user.IsActive, user.CreatedAt, user.CreatedBy);
    }
}
