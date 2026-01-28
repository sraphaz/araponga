using Araponga.Application.Services;
using Araponga.Modules.Core;
using Araponga.Modules.Moderation.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Moderation;

/// <summary>
/// Módulo Moderation - serviços de moderação e governança.
/// Depende de Core.
/// </summary>
public sealed class ModerationModule : ModuleBase
{
    public override string Id => "Moderation";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Moderation
        services.AddModerationInfrastructure(configuration);

        services.AddScoped<ReportService>();
        services.AddScoped<UserBlockService>();
        services.AddScoped<FeatureFlagService>();
        services.AddScoped<TerritoryModerationService>();
        services.AddScoped<ModerationCaseService>();
        services.AddScoped<InterestFilterService>();
        services.AddScoped<TerritoryCharacterizationService>();
        services.AddScoped<SystemPermissionService>();
        services.AddScoped<MembershipCapabilityService>();
        services.AddScoped<VerificationQueueService>();
        services.AddScoped<DocumentEvidenceService>();
        services.AddScoped<VotingService>();
        services.AddScoped<UserInterestService>();
    }
}
