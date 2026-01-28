using Araponga.Application.Interfaces;
using Araponga.Modules.Map.Infrastructure.Postgres;
using Araponga.Modules.Map.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Map.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar MapDbContext
        services.AddDbContext<MapDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Map
        services.AddScoped<IMapRepository, PostgresMapRepository>();
        services.AddScoped<IMapEntityRelationRepository, PostgresMapEntityRelationRepository>();

        return services;
    }
}
