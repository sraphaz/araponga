using Araponga.Application.Interfaces;
using Araponga.Modules.Subscriptions.Infrastructure.Postgres;
using Araponga.Modules.Subscriptions.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Subscriptions.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSubscriptionsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar SubscriptionsDbContext
        services.AddDbContext<SubscriptionsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Subscriptions
        services.AddScoped<ISubscriptionRepository, PostgresSubscriptionRepository>();
        services.AddScoped<ISubscriptionPlanRepository, PostgresSubscriptionPlanRepository>();

        return services;
    }
}
