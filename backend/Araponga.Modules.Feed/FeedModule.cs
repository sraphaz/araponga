using Araponga.Application.Services;
using Araponga.Modules.Core;
using Araponga.Modules.Feed.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Feed;

/// <summary>
/// Módulo Feed - serviços de feed comunitário.
/// Depende de Core.
/// </summary>
public sealed class FeedModule : ModuleBase
{
    public override string Id => "Feed";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Feed
        services.AddFeedInfrastructure(configuration);

        // Registrar services do módulo Feed
        services.AddScoped<Araponga.Modules.Feed.Application.Services.PostCreationService>();
        services.AddScoped<Araponga.Modules.Feed.Application.Services.PostEditService>();
        services.AddScoped<Araponga.Modules.Feed.Application.Services.PostInteractionService>();
        services.AddScoped<Araponga.Modules.Feed.Application.Services.PostFilterService>();
        services.AddScoped<Araponga.Modules.Feed.Application.Interfaces.IFeedService, Araponga.Modules.Feed.Application.Services.FeedService>();
        
        // Manter services antigos por compatibilidade temporária (serão removidos após migração completa)
        services.AddScoped<Araponga.Application.Services.PostCreationService>();
        services.AddScoped<Araponga.Application.Services.PostEditService>();
        services.AddScoped<Araponga.Application.Services.PostInteractionService>();
        services.AddScoped<Araponga.Application.Services.PostFilterService>();
        services.AddScoped<Araponga.Application.Services.FeedService>();
    }
}
