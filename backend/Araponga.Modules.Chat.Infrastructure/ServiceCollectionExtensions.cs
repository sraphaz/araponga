using Araponga.Application.Interfaces;
using Araponga.Modules.Chat.Infrastructure.Postgres;
using Araponga.Modules.Chat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Modules.Chat.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChatInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar ChatDbContext
        services.AddDbContext<ChatDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found in configuration.");
            }

            options.UseNpgsql(connectionString);
        });

        // Registrar repositórios de Chat
        services.AddScoped<IChatConversationRepository, PostgresChatConversationRepository>();
        services.AddScoped<IChatConversationParticipantRepository, PostgresChatConversationParticipantRepository>();
        services.AddScoped<IChatMessageRepository, PostgresChatMessageRepository>();
        services.AddScoped<IChatConversationStatsRepository, PostgresChatConversationStatsRepository>();

        return services;
    }
}
