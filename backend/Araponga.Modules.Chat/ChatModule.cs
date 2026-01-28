using Araponga.Application.Services;
using Araponga.Modules.Chat.Infrastructure;
using Araponga.Modules.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Chat;

/// <summary>
/// Módulo Chat - serviços de chat e mensagens.
/// Depende de Core.
/// </summary>
public sealed class ChatModule : ModuleBase
{
    public override string Id => "Chat";
    public override string[] DependsOn => new[] { "Core" };
    public override bool IsRequired => false;

    public override void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrar infraestrutura do módulo Chat
        services.AddChatInfrastructure(configuration);

        services.AddScoped<ChatService>();
    }
}
