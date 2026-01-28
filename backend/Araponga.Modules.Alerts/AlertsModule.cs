using Araponga.Modules.Alerts.Infrastructure;
using Araponga.Modules.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Alerts;

/// <summary>
/// Módulo Alerts - serviços de alertas.
/// Depende de Core.
/// </summary>
public sealed class AlertsModule : ModuleBase
{
    public override string Id => "Alerts";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Alerts
        services.AddAlertsInfrastructure(configuration);

        // Serviços específicos de alertas serão adicionados aqui quando implementados
    }
}
