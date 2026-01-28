using Araponga.Application.Interfaces;
using Araponga.Modules.Alerts.Infrastructure.Postgres;
using Araponga.Modules.Alerts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Alerts.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAlertsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar AlertsDbContext
        services.AddDbContext<AlertsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Alerts
        services.AddScoped<IHealthAlertRepository, PostgresHealthAlertRepository>();

        return services;
    }
}
