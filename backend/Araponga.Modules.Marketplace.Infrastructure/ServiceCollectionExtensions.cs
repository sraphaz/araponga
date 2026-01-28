using Araponga.Application.Interfaces;
using Araponga.Modules.Marketplace.Infrastructure.Postgres;
using Araponga.Modules.Marketplace.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Marketplace.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarketplaceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar MarketplaceDbContext
        services.AddDbContext<MarketplaceDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Marketplace
        services.AddScoped<IStoreRepository, PostgresStoreRepository>();
        services.AddScoped<IStoreItemRepository, PostgresStoreItemRepository>();
        services.AddScoped<ICartRepository, PostgresCartRepository>();
        services.AddScoped<IInquiryRepository, PostgresInquiryRepository>();

        return services;
    }
}
