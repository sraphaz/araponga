using Araponga.Application.Interfaces;
using Araponga.Application.Services;
using Araponga.Infrastructure.InMemory;
using Araponga.Tests.TestHelpers;

namespace Araponga.Tests.TestHelpers.Services;

public static class InquiryServiceTestHelper
{
    public static InquiryService CreateService(
        InMemoryDataStore dataStore,
        TerritoryFeatureFlagGuard? featureGuard = null,
        IUnitOfWork? unitOfWork = null)
    {
        if (featureGuard == null)
        {
            var featureFlags = new InMemoryFeatureFlagService();
            var cache = CacheTestHelper.CreateDistributedCacheService();
            var featureFlagCache = new FeatureFlagCacheService(featureFlags, cache);
            featureGuard = new TerritoryFeatureFlagGuard(featureFlagCache);
        }

        return new InquiryService(
            new InMemoryInquiryRepository(dataStore),
            new InMemoryStoreItemRepository(dataStore),
            new InMemoryStoreRepository(dataStore),
            featureGuard,
            unitOfWork ?? new InMemoryUnitOfWork());
    }
}
