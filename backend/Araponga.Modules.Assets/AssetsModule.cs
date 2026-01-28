using Araponga.Application.Services;
using Araponga.Modules.Assets.Infrastructure;
using Araponga.Modules.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Assets;

/// <summary>
/// Módulo Assets - serviços de assets e mídia.
/// Depende de Core.
/// </summary>
public sealed class AssetsModule : ModuleBase
{
    public override string Id => "Assets";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Assets
        services.AddAssetsInfrastructure(configuration);

        services.AddScoped<MediaService>();
        // Nota: IGlobalMediaLimits é registrado em AddSharedApplicationServices (está no Api)
        services.AddScoped<Araponga.Application.Services.Media.TerritoryMediaConfigService>();
        services.AddScoped<Araponga.Application.Services.Users.UserMediaPreferencesService>();
        services.AddScoped<Araponga.Application.Services.Media.MediaStorageConfigService>();
    }
}
