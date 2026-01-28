using Araponga.Application.Interfaces;
using Araponga.Application.Interfaces.Media;
using Araponga.Modules.Assets.Infrastructure.Postgres;
using Araponga.Modules.Assets.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Assets.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAssetsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar AssetsDbContext
        services.AddDbContext<AssetsDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Assets", "public");
            });
        });

        // Registrar repositórios de Assets
        services.AddScoped<ITerritoryAssetRepository, PostgresAssetRepository>();
        services.AddScoped<IAssetGeoAnchorRepository, PostgresAssetGeoAnchorRepository>();
        services.AddScoped<IAssetValidationRepository, PostgresAssetValidationRepository>();
        services.AddScoped<IPostAssetRepository, PostgresPostAssetRepository>();

        // Registrar repositórios de Media
        services.AddScoped<IMediaAssetRepository, PostgresMediaAssetRepository>();
        services.AddScoped<IMediaAttachmentRepository, PostgresMediaAttachmentRepository>();

        return services;
    }
}
