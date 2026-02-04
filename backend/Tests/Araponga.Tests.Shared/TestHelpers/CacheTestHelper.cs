using Araponga.Application.Interfaces;
using Araponga.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace Araponga.Tests.Shared.TestHelpers;

/// <summary>Helper compartilhado para criar serviços de cache nos testes.</summary>
public static class CacheTestHelper
{
    public static IDistributedCacheService CreateDistributedCacheService()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var logger = NullLogger<RedisCacheService>.Instance;
        return new RedisCacheService(null, memoryCache, logger);
    }

    public static IDistributedCacheService CreatePatternAwareCacheService()
        => new PatternAwareTestCacheService();
}
