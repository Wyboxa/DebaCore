using Debales.Application.Core.Users.Commands.CreateUser;
using Debales.Application.Core.Users.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace Debales.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<GetUserByIdHandler>();

        return services;
    }
}
