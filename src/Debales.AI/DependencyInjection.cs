using Debales.AI.Providers;
using Debales.AI.Services;
using Debales.Application.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Debales.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddAI(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AISettings>(configuration.GetSection("AI"));

        var settings = configuration.GetSection("AI").Get<AISettings>() ?? new AISettings();

        if (settings.Provider.Equals("Mock", StringComparison.OrdinalIgnoreCase))
        {
            services.AddSingleton<IAIProvider, MockAIProvider>();
        }
        else
        {
            services.AddHttpClient<IAIProvider, ClaudeProvider>(client =>
            {
                client.BaseAddress = new Uri("https://api.anthropic.com/v1/");
                client.DefaultRequestHeaders.Add("x-api-key", settings.ApiKey);
                client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            });
        }

        services.AddScoped<IAIService, AIService>();

        return services;
    }
}
