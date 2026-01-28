using Araponga.Application.Services;
using Araponga.Modules.Core;
using Araponga.Modules.Map.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Map;

/// <summary>
/// Módulo Map - serviços de mapa e entidades geográficas.
/// Depende de Core.
/// </summary>
public sealed class MapModule : ModuleBase
{
    public override string Id => "Map";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Map
        services.AddMapInfrastructure(configuration);

        services.AddScoped<MapService>();
        services.AddScoped<TerritoryAssetService>();
    }
}
