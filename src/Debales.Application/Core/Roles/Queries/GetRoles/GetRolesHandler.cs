using Debales.Application.Core.Roles.DTOs;

namespace Debales.Application.Core.Roles.Queries.GetRoles;

public sealed class GetRolesHandler
{
    private readonly IRoleRepository _roles;
    public GetRolesHandler(IRoleRepository roles) => _roles = roles;

    public async Task<IReadOnlyList<RoleDto>> Handle(CancellationToken cancellationToken = default)
    {
        var roles = await _roles.GetAllAsync(cancellationToken);
        return roles.Select(r => new RoleDto(r.Id, r.Name, r.Description, r.IsSystem))
                    .OrderBy(r => r.Name)
                    .ToList();
    }
}
