using Araponga.Application.Services;
using Araponga.Modules.Admin.Infrastructure;
using Araponga.Modules.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Admin;

/// <summary>
/// Módulo Admin - serviços administrativos e de sistema.
/// Depende de Core.
/// </summary>
public sealed class AdminModule : ModuleBase
{
    public override string Id => "Admin";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Admin
        services.AddAdminInfrastructure(configuration);

        services.AddScoped<SystemConfigCacheService>();
        services.AddScoped<SystemConfigService>();
        services.AddScoped<WorkQueueService>();
        services.AddScoped<AnalyticsService>();
        services.AddScoped<InputSanitizationService>();
    }
}
