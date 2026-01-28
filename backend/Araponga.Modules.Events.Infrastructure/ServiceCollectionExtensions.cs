using Araponga.Application.Interfaces;
using Araponga.Modules.Events.Infrastructure.Postgres;
using Araponga.Modules.Events.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Events.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar EventsDbContext
        services.AddDbContext<EventsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Events
        services.AddScoped<ITerritoryEventRepository, PostgresTerritoryEventRepository>();
        services.AddScoped<IEventParticipationRepository, PostgresEventParticipationRepository>();

        return services;
    }
}
