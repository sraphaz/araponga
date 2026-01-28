using Araponga.Application.Interfaces;
using Araponga.Modules.Admin.Infrastructure.Postgres;
using Araponga.Modules.Admin.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Admin.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar AdminDbContext
        services.AddDbContext<AdminDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Admin", "public");
            });
        });

        // Registrar repositórios de Admin
        services.AddScoped<IWorkItemRepository, PostgresWorkItemRepository>();
        services.AddScoped<IDocumentEvidenceRepository, PostgresDocumentEvidenceRepository>();

        return services;
    }
}
