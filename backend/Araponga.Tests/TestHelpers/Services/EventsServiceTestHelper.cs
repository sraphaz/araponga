using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Application.Services.Media;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers.Dependencies;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de EventsService em testes.
/// </summary>
public static class EventsServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de EventsService com todas as dependências necessárias.
    /// </summary>
    public static EventsService CreateService(
        InMemoryDataStore dataStore,
        AccessEvaluator? accessEvaluator = null,
        TerritoryMediaConfigService? mediaConfigService = null,
        EventCacheService? eventCache = null,
        CacheInvalidationService? cacheInvalidation = null)
    {
        var eventRepository = new InMemoryTerritoryEventRepository(dataStore);
        var participationRepository = new InMemoryEventParticipationRepository(dataStore);
        var feedRepository = new InMemoryFeedRepository(dataStore);
        var mediaAssetRepository = new InMemoryMediaAssetRepository(dataStore);
        var mediaAttachmentRepository = new InMemoryMediaAttachmentRepository(dataStore);
        var userRepository = new InMemoryUserRepository(dataStore);
        var unitOfWork = new InMemoryUnitOfWork();
        
        var accessEvaluatorInstance = accessEvaluator ?? AccessEvaluatorTestHelper.CreateAccessEvaluator(dataStore);
        
        var featureFlags = new InMemoryFeatureFlagService();
        var mediaConfigServiceInstance = mediaConfigService ?? new TerritoryMediaConfigService(
            new InMemoryTerritoryMediaConfigRepository(dataStore),
            featureFlags,
            unitOfWork,
            new InMemoryGlobalMediaLimits());

        return new EventsService(
            eventRepository,
            participationRepository,
            feedRepository,
            mediaAssetRepository,
            mediaAttachmentRepository,
            mediaConfigServiceInstance,
            accessEvaluatorInstance,
            userRepository,
            unitOfWork,
            eventCache,
            cacheInvalidation);
    }
}
