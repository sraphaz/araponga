using Araponga.Modules.Core;
using Araponga.Modules.CoreModule;
using Araponga.Modules.Feed;
using Araponga.Modules.Marketplace;
using Araponga.Modules.Subscriptions;
using Araponga.Modules.Chat;
using Araponga.Modules.Events;
using Araponga.Modules.Map;
using Araponga.Modules.Moderation;
using Araponga.Modules.Notifications;
using Araponga.Modules.Alerts;
using Araponga.Modules.Assets;
using Araponga.Modules.Admin;
using Araponga.Api.Extensions;
using Araponga.Application.Interfaces;
using Araponga.Application.Interfaces.Media;
using Araponga.Application.Interfaces.Users;
using Araponga.Infrastructure.InMemory;
using Araponga.Infrastructure.Caching;
using Araponga.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace Araponga.Tests.TestHelpers;

/// <summary>
/// Factory genérica para criar serviços em testes usando composição baseada em módulos.
/// Usa o mesmo pipeline de registro que o host (shared + módulos), garantindo que testes
/// "vejam" o mesmo DI que a aplicação.
/// </summary>
public class ServiceTestFactory<TService> where TService : class
{
    private readonly ITestServiceCollection _config;
    private readonly IConfiguration _configuration;

    public ServiceTestFactory(ITestServiceCollection config, IConfiguration? configuration = null)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _configuration = configuration ?? CreateDefaultConfiguration();
    }

    /// <summary>
    /// Cria uma instância do serviço usando o pipeline de módulos.
    /// </summary>
    public TService CreateService()
    {
        var services = new ServiceCollection();
        _config.ConfigureServices(services, _configuration);
        var provider = _config.BuildServiceProvider(services);
        return provider.GetRequiredService<TService>();
    }

    /// <summary>
    /// Cria o ServiceProvider completo (útil para testes que precisam de múltiplos serviços).
    /// </summary>
    public IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        _config.ConfigureServices(services, _configuration);
        return _config.BuildServiceProvider(services);
    }

    private static IConfiguration CreateDefaultConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Modules:Core:Enabled", "true" },
                { "Modules:Core:Required", "true" },
                { "Modules:Feed:Enabled", "true" },
                { "Modules:Marketplace:Enabled", "true" },
                { "Modules:Subscriptions:Enabled", "true" },
                { "Modules:Chat:Enabled", "true" },
                { "Modules:Events:Enabled", "true" },
                { "Modules:Map:Enabled", "true" },
                { "Modules:Moderation:Enabled", "true" },
                { "Modules:Notifications:Enabled", "true" },
                { "Modules:Alerts:Enabled", "true" },
                { "Modules:Assets:Enabled", "true" },
                { "Modules:Admin:Enabled", "true" }
            })
            .Build();
    }
}

/// <summary>
/// Implementação padrão de ITestServiceCollection que usa o mesmo pipeline do host.
/// </summary>
public class DefaultTestServiceCollection : ITestServiceCollection
{
    private readonly InMemoryDataStore _dataStore;

    public DefaultTestServiceCollection(InMemoryDataStore? dataStore = null)
    {
        _dataStore = dataStore ?? new InMemoryDataStore();
    }

    public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Substituir InMemoryDataStore se já estiver registrado
        var existingDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(InMemoryDataStore));
        if (existingDescriptor != null)
        {
            services.Remove(existingDescriptor);
        }
        services.AddSingleton(_dataStore);

        // Adicionar infraestrutura InMemory (simula AddInfrastructure com InMemory)
        services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();
        
        // Registrar repositórios InMemory manualmente (AddInMemoryRepositories é privado)
        services.AddSingleton<ITerritoryRepository, InMemoryTerritoryRepository>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<ITerritoryMembershipRepository, InMemoryTerritoryMembershipRepository>();
        services.AddSingleton<ITerritoryJoinRequestRepository, InMemoryTerritoryJoinRequestRepository>();
        services.AddSingleton<IFeedRepository, InMemoryFeedRepository>();
        services.AddSingleton<ITerritoryEventRepository, InMemoryTerritoryEventRepository>();
        services.AddSingleton<IEventParticipationRepository, InMemoryEventParticipationRepository>();
        services.AddSingleton<IMapRepository, InMemoryMapRepository>();
        services.AddSingleton<IMapEntityRelationRepository, InMemoryMapEntityRelationRepository>();
        services.AddSingleton<IPostGeoAnchorRepository, InMemoryPostGeoAnchorRepository>();
        services.AddSingleton<ITerritoryAssetRepository, InMemoryAssetRepository>();
        services.AddSingleton<IAssetGeoAnchorRepository, InMemoryAssetGeoAnchorRepository>();
        services.AddSingleton<IAssetValidationRepository, InMemoryAssetValidationRepository>();
        services.AddSingleton<IPostAssetRepository, InMemoryPostAssetRepository>();
        services.AddSingleton<IActiveTerritoryStore, InMemoryActiveTerritoryStore>();
        services.AddSingleton<IHealthAlertRepository, InMemoryHealthAlertRepository>();
        services.AddSingleton<IFeatureFlagService, InMemoryFeatureFlagService>();
        services.AddSingleton<IAuditLogger, InMemoryAuditLogger>();
        services.AddSingleton<IReportRepository, InMemoryReportRepository>();
        services.AddSingleton<IUserBlockRepository, InMemoryUserBlockRepository>();
        services.AddSingleton<ISanctionRepository, InMemorySanctionRepository>();
        services.AddSingleton<IOutbox, InMemoryOutbox>();
        services.AddSingleton<INotificationInboxRepository, InMemoryNotificationInboxRepository>();
        services.AddSingleton<IStoreRepository, InMemoryStoreRepository>();
        services.AddSingleton<IStoreItemRepository, InMemoryStoreItemRepository>();
        services.AddSingleton<IInquiryRepository, InMemoryInquiryRepository>();
        services.AddSingleton<IStoreRatingRepository, InMemoryStoreRatingRepository>();
        services.AddSingleton<IStoreItemRatingRepository, InMemoryStoreItemRatingRepository>();
        services.AddSingleton<IStoreRatingResponseRepository, InMemoryStoreRatingResponseRepository>();
        services.AddSingleton<ICartRepository, InMemoryCartRepository>();
        services.AddSingleton<ICartItemRepository, InMemoryCartItemRepository>();
        services.AddSingleton<ICheckoutRepository, InMemoryCheckoutRepository>();
        services.AddSingleton<ICheckoutItemRepository, InMemoryCheckoutItemRepository>();
        services.AddSingleton<IPlatformFeeConfigRepository, InMemoryPlatformFeeConfigRepository>();
        services.AddSingleton<ITerritoryPayoutConfigRepository, InMemoryTerritoryPayoutConfigRepository>();
        services.AddSingleton<IFinancialTransactionRepository, InMemoryFinancialTransactionRepository>();
        services.AddSingleton<ITransactionStatusHistoryRepository, InMemoryTransactionStatusHistoryRepository>();
        services.AddSingleton<ISellerBalanceRepository, InMemorySellerBalanceRepository>();
        services.AddSingleton<ISellerTransactionRepository, InMemorySellerTransactionRepository>();
        services.AddSingleton<IPlatformFinancialBalanceRepository, InMemoryPlatformFinancialBalanceRepository>();
        services.AddSingleton<IPlatformRevenueTransactionRepository, InMemoryPlatformRevenueTransactionRepository>();
        services.AddSingleton<IPlatformExpenseTransactionRepository, InMemoryPlatformExpenseTransactionRepository>();
        services.AddSingleton<IReconciliationRecordRepository, InMemoryReconciliationRecordRepository>();
        services.AddSingleton<IUserPreferencesRepository, InMemoryUserPreferencesRepository>();
        services.AddSingleton<IUserInterestRepository, InMemoryUserInterestRepository>();
        services.AddSingleton<IVotingRepository, InMemoryVotingRepository>();
        services.AddSingleton<IVoteRepository, InMemoryVoteRepository>();
        services.AddSingleton<ITerritoryModerationRuleRepository, InMemoryTerritoryModerationRuleRepository>();
        services.AddSingleton<ITerritoryCharacterizationRepository, InMemoryTerritoryCharacterizationRepository>();
        services.AddSingleton<Araponga.Application.Interfaces.Notifications.INotificationConfigRepository, InMemoryNotificationConfigRepository>();
        services.AddSingleton<IMembershipSettingsRepository, InMemoryMembershipSettingsRepository>();
        services.AddSingleton<IMembershipCapabilityRepository, InMemoryMembershipCapabilityRepository>();
        services.AddSingleton<ISystemPermissionRepository, InMemorySystemPermissionRepository>();
        services.AddSingleton<ISystemConfigRepository, InMemorySystemConfigRepository>();
        services.AddSingleton<IWorkItemRepository, InMemoryWorkItemRepository>();
        services.AddSingleton<IDocumentEvidenceRepository, InMemoryDocumentEvidenceRepository>();
        services.AddSingleton<IChatConversationRepository, InMemoryChatConversationRepository>();
        services.AddSingleton<IChatConversationParticipantRepository, InMemoryChatConversationParticipantRepository>();
        services.AddSingleton<IChatMessageRepository, InMemoryChatMessageRepository>();
        services.AddSingleton<IChatConversationStatsRepository, InMemoryChatConversationStatsRepository>();
        services.AddSingleton<IMediaAssetRepository, InMemoryMediaAssetRepository>();
        services.AddSingleton<IMediaAttachmentRepository, InMemoryMediaAttachmentRepository>();
        services.AddSingleton<ITerritoryMediaConfigRepository, InMemoryTerritoryMediaConfigRepository>();
        services.AddSingleton<IUserMediaPreferencesRepository, InMemoryUserMediaPreferencesRepository>();
        services.AddSingleton<IMediaStorageConfigRepository, InMemoryMediaStorageConfigRepository>();
        services.AddSingleton<ITermsOfServiceRepository, InMemoryTermsOfServiceRepository>();
        services.AddSingleton<ITermsAcceptanceRepository, InMemoryTermsAcceptanceRepository>();
        services.AddSingleton<IPrivacyPolicyRepository, InMemoryPrivacyPolicyRepository>();
        services.AddSingleton<IPrivacyPolicyAcceptanceRepository, InMemoryPrivacyPolicyAcceptanceRepository>();
        services.AddSingleton<ISubscriptionPlanRepository, InMemorySubscriptionPlanRepository>();
        services.AddSingleton<ISubscriptionRepository, InMemorySubscriptionRepository>();
        services.AddSingleton<ISubscriptionPaymentRepository, InMemorySubscriptionPaymentRepository>();
        services.AddSingleton<ICouponRepository, InMemoryCouponRepository>();
        services.AddSingleton<ISubscriptionCouponRepository, InMemorySubscriptionCouponRepository>();
        services.AddSingleton<ISubscriptionPlanHistoryRepository, InMemorySubscriptionPlanHistoryRepository>();
        services.AddSingleton<IUserDeviceRepository, InMemoryUserDeviceRepository>();

        // Adicionar serviços de infraestrutura necessários
        services.AddSingleton<Araponga.Application.Interfaces.IObservabilityLogger, InMemoryObservabilityLogger>();
        services.AddSingleton<ITokenService, Araponga.Infrastructure.Security.JwtTokenService>();
        services.AddSingleton<Araponga.Infrastructure.Security.ISecretsService, Araponga.Infrastructure.Security.EnvironmentSecretsService>();
        services.AddSingleton<IFileStorage>(_ => new Araponga.Infrastructure.FileStorage.LocalFileStorage(Path.Combine(AppContext.BaseDirectory, "app_data", "uploads")));
        
        // Cache em memória para testes
        services.AddMemoryCache();
        services.AddSingleton<IDistributedCache, MemoryDistributedCache>();
        services.AddSingleton<IDistributedCacheService>(sp =>
        {
            var distributedCache = sp.GetService<IDistributedCache>();
            var memoryCache = sp.GetRequiredService<IMemoryCache>();
            var logger = sp.GetRequiredService<ILogger<RedisCacheService>>();
            return new RedisCacheService(distributedCache, memoryCache, logger);
        });

        // Media storage (mínimo para testes)
        services.Configure<Araponga.Infrastructure.Media.MediaStorageOptions>(configuration.GetSection("MediaStorage"));
        services.AddSingleton<Araponga.Infrastructure.Media.MediaStorageFactory>();
        services.AddScoped<Araponga.Application.Interfaces.Media.IMediaStorageService>(sp =>
        {
            var factory = sp.GetRequiredService<Araponga.Infrastructure.Media.MediaStorageFactory>();
            return factory.CreateStorageService();
        });
        services.AddScoped<Araponga.Application.Interfaces.Media.IMediaProcessingService, Araponga.Infrastructure.Media.LocalMediaProcessingService>();
        services.AddScoped<Araponga.Application.Interfaces.Media.IMediaValidator, Araponga.Infrastructure.Media.MediaValidator>();
        services.AddSingleton<Araponga.Application.Interfaces.Media.IAsyncMediaProcessor, Araponga.Infrastructure.Media.NoOpAsyncMediaProcessor>();

        // Email queue InMemory
        services.AddSingleton<IEmailQueueRepository>(sp => new InMemoryEmailQueueRepository(_dataStore));

        // Adicionar event handlers (InMemory)
        services.AddEventHandlers();

        // Adicionar conectores compartilhados
        services.AddConnectors(configuration);

        // Adicionar serviços compartilhados
        services.AddSharedApplicationServices(configuration);

        // Registrar módulos
        var modules = new IModule[]
        {
            new Araponga.Modules.CoreModule.CoreModule(),
            new Araponga.Modules.Feed.FeedModule(),
            new Araponga.Modules.Marketplace.MarketplaceModule(),
            new Araponga.Modules.Subscriptions.SubscriptionsModule(),
            new Araponga.Modules.Chat.ChatModule(),
            new Araponga.Modules.Events.EventsModule(),
            new Araponga.Modules.Map.MapModule(),
            new Araponga.Modules.Moderation.ModerationModule(),
            new Araponga.Modules.Notifications.NotificationsModule(),
            new Araponga.Modules.Alerts.AlertsModule(),
            new Araponga.Modules.Assets.AssetsModule(),
            new Araponga.Modules.Admin.AdminModule()
        };

        // Aplicar módulos
        var loggerFactory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
        var moduleRegistry = new ModuleRegistry(modules, loggerFactory.CreateLogger<ModuleRegistry>());
        moduleRegistry.Apply(services, configuration);
        services.AddSingleton<IModuleRegistry>(moduleRegistry);

        // Adicionar logging mínimo para testes
        services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));

        return services;
    }

    public IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}
