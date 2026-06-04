using Debales.Application.Core.Roles;
using Debales.Application.Core.Users.DTOs;

namespace Debales.Application.Core.Users.Queries.GetUsers;

public sealed class GetUsersHandler
{
    private readonly IUserRepository _users;
    private readonly IRoleRepository _roles;

    public GetUsersHandler(IUserRepository users, IRoleRepository roles)
    {
        _users = users;
        _roles = roles;
    }

    public async Task<IReadOnlyList<UserSummaryDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken = default)
    {
        var users = await _users.GetAllWithRolesAsync(cancellationToken);
        var allRoles = await _roles.GetAllAsync(cancellationToken);
        var roleMap = allRoles.ToDictionary(r => r.Id, r => r.Name);

        IEnumerable<Debales.Domain.Core.Users.User> filtered = users;

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLowerInvariant();
            filtered = filtered.Where(u =>
                u.Username.ToLowerInvariant().Contains(term) ||
                u.Email.Value.ToLowerInvariant().Contains(term));
        }

        if (query.IsActive.HasValue)
            filtered = filtered.Where(u => u.IsActive == query.IsActive.Value);

        return filtered
            .OrderBy(u => u.Username)
            .Select(u => new UserSummaryDto(
                u.Id,
                u.Username,
                u.Email.Value,
                u.IsActive,
                u.UserRoles.Select(ur => roleMap.TryGetValue(ur.RoleId, out var name) ? name : "?").ToList(),
                u.CreatedAt))
            .ToList();
    }
}
