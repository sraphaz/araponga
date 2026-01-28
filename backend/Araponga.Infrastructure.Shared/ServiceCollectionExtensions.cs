using Araponga.Application.Events;
using Araponga.Application.Interfaces;
using Araponga.Infrastructure.Shared.Postgres;
using Araponga.Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Araponga.Infrastructure.Shared;

/// <summary>
/// Extensões para registrar infraestrutura compartilhada.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra o SharedDbContext e repositórios compartilhados.
    /// </summary>
    public static IServiceCollection AddSharedInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var persistenceProvider = configuration.GetValue<string>("Persistence:Provider") ?? "InMemory";

        if (string.Equals(persistenceProvider, "Postgres", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<SharedDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres"), npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                    npgsqlOptions.CommandTimeout(30);
                }));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SharedDbContext>());
            services.AddSharedRepositories();
        }

        return services;
    }

    /// <summary>
    /// Registra repositórios compartilhados (Territory, User, Membership, etc.).
    /// </summary>
    private static IServiceCollection AddSharedRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITerritoryRepository, PostgresTerritoryRepository>();
        services.AddScoped<IUserRepository, PostgresUserRepository>();
        services.AddScoped<ITerritoryMembershipRepository, PostgresTerritoryMembershipRepository>();
        services.AddScoped<IUserPreferencesRepository, PostgresUserPreferencesRepository>();
        services.AddScoped<IUserDeviceRepository, PostgresUserDeviceRepository>();
        services.AddScoped<IUserInterestRepository, PostgresUserInterestRepository>();
        services.AddScoped<IMembershipSettingsRepository, PostgresMembershipSettingsRepository>();
        services.AddScoped<IMembershipCapabilityRepository, PostgresMembershipCapabilityRepository>();
        services.AddScoped<ISystemPermissionRepository, PostgresSystemPermissionRepository>();
        services.AddScoped<ISystemConfigRepository, PostgresSystemConfigRepository>();

        return services;
    }

    /// <summary>
    /// Registra serviços cross-cutting compartilhados (Cache, Email, EventBus, Outbox, Audit).
    /// </summary>
    public static IServiceCollection AddSharedCrossCuttingServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Cache
        services.AddScoped<IDistributedCacheService, Services.RedisCacheService>();

        // Email
        services.Configure<Services.EmailConfiguration>(configuration.GetSection("Email"));
        services.AddScoped<IEmailSender, Services.SmtpEmailSender>();

        // EventBus
        services.AddScoped<IEventBus, Services.InMemoryEventBus>();

        // Outbox
        services.AddScoped<IOutbox, Services.PostgresOutbox>();

        // Audit
        services.AddScoped<IAuditLogger, Services.PostgresAuditLogger>();

        return services;
    }
}
