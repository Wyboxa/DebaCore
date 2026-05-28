using Debales.Application.Common;
using Debales.Domain.Core.Roles;
using Debales.Domain.Core.Users;

namespace Debales.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        await SeedRolesAsync(context);
        await SeedAdminUserAsync(context, passwordHasher);
    }

    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (context.Roles.Any()) return;

        context.Roles.Add(Role.Create("Admin", "Administrador del sistema", "seed", isSystem: true));
        context.Roles.Add(Role.Create("User", "Usuario estándar", "seed", isSystem: true));
        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminUserAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        if (context.Users.Any()) return;

        var email = Email.Create("admin@debales.local");
        var hash = passwordHasher.Hash("Admin1234!");
        var admin = User.Create("admin", email, hash, "seed");

        var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole is not null)
            admin.AssignRole(adminRole);

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
