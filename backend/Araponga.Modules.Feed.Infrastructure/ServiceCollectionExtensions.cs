using Araponga.Application.Interfaces;
using Araponga.Modules.Feed.Infrastructure.Postgres;
using Araponga.Modules.Feed.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Feed.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFeedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar FeedDbContext
        services.AddDbContext<FeedDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Feed
        services.AddScoped<IFeedRepository, PostgresFeedRepository>();

        return services;
    }
}
