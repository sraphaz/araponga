using Araponga.Application.Services;
using Araponga.Modules.Core;
using Araponga.Modules.Events.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Events;

/// <summary>
/// Módulo Events - serviços de eventos comunitários.
/// Depende de Core. Pode integrar com Feed (ex.: eventos no feed).
/// </summary>
public sealed class EventsModule : ModuleBase
{
    public override string Id => "Events";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Events
        services.AddEventsInfrastructure(configuration);

        services.AddScoped<EventsService>();
    }
}
