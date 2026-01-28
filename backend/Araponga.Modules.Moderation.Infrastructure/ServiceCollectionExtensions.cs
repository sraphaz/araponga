using Araponga.Application.Interfaces;
using Araponga.Modules.Moderation.Infrastructure.Postgres;
using Araponga.Modules.Moderation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Moderation.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModerationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar ModerationDbContext
        services.AddDbContext<ModerationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Moderation
        services.AddScoped<IReportRepository, PostgresReportRepository>();
        services.AddScoped<IUserBlockRepository, PostgresUserBlockRepository>();
        services.AddScoped<ISanctionRepository, PostgresSanctionRepository>();

        return services;
    }
}
