using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Application.Services.Media;
using Araponga.Infrastructure.InMemory;

namespace Araponga.Tests.TestHelpers.Services;

/// <summary>
/// Helper para criar instâncias de PostEditService em testes.
/// </summary>
public static class PostEditServiceTestHelper
{
    /// <summary>
    /// Cria uma instância de PostEditService com todas as dependências necessárias.
    /// </summary>
    public static PostEditService CreateService(
        InMemoryDataStore dataStore,
        IFeatureFlagService? featureFlags = null,
        TerritoryMediaConfigService? mediaConfigService = null,
        CacheInvalidationService? cacheInvalidation = null)
    {
        var feedRepository = new InMemoryFeedRepository(dataStore);
        var mediaAttachmentRepository = new InMemoryMediaAttachmentRepository(dataStore);
        var mediaAssetRepository = new InMemoryMediaAssetRepository(dataStore);
        var geoAnchorRepository = new InMemoryPostGeoAnchorRepository(dataStore);
        var featureFlagsInstance = featureFlags ?? new InMemoryFeatureFlagService();
        var mediaConfigServiceInstance = mediaConfigService ?? new TerritoryMediaConfigService(
            new InMemoryTerritoryMediaConfigRepository(dataStore),
            featureFlagsInstance,
            new InMemoryUnitOfWork(),
            new InMemoryGlobalMediaLimits());
        var unitOfWork = new InMemoryUnitOfWork();

        return new PostEditService(
            feedRepository,
            mediaAttachmentRepository,
            mediaAssetRepository,
            geoAnchorRepository,
            featureFlagsInstance,
            mediaConfigServiceInstance,
            unitOfWork,
            cacheInvalidation);
    }
}
