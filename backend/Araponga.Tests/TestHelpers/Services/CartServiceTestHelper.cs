using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Araponga.Tests.TestHelpers.Services;

public static class CartServiceTestHelper
{
    public static CartService CreateService(
        InMemoryDataStore dataStore,
        TerritoryFeatureFlagGuard? featureGuard = null,
        IUnitOfWork? unitOfWork = null,
        IServiceProvider? serviceProvider = null,
        List<(Guid TerritoryId, List<FeatureFlag> Flags)>? territoryFeatureFlags = null)
    {
        if (featureGuard == null)
        {
            var featureFlags = new InMemoryFeatureFlagService();
            
            // Configurar feature flags por território se fornecido
            if (territoryFeatureFlags != null)
            {
                foreach (var (territoryId, flags) in territoryFeatureFlags)
                {
                    featureFlags.SetEnabledFlags(territoryId, flags);
                }
            }
            
            var cache = CacheTestHelper.CreateDistributedCacheService();
            var featureFlagCache = new FeatureFlagCacheService(featureFlags, cache);
            featureGuard = new TerritoryFeatureFlagGuard(featureFlagCache);
        }

        return new CartService(
            new InMemoryCartRepository(dataStore),
            new InMemoryCartItemRepository(dataStore),
            new InMemoryStoreItemRepository(dataStore),
            new InMemoryStoreRepository(dataStore),
            new InMemoryCheckoutRepository(dataStore),
            new InMemoryCheckoutItemRepository(dataStore),
            new InMemoryInquiryRepository(dataStore),
            new InMemoryPlatformFeeConfigRepository(dataStore),
            featureGuard,
            unitOfWork ?? new InMemoryUnitOfWork(),
            serviceProvider);
    }
}
