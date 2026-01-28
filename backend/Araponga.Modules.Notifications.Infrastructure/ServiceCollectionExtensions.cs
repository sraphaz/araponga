using Araponga.Application.Interfaces;
using Araponga.Modules.Notifications.Infrastructure.Postgres;
using Araponga.Modules.Notifications.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Notifications.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar NotificationsDbContext
        services.AddDbContext<NotificationsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Notifications
        services.AddScoped<INotificationInboxRepository, PostgresNotificationInboxRepository>();

        return services;
    }
}
