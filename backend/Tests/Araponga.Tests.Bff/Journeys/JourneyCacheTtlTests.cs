using Araponga.Bff.Journeys;
using Araponga.Bff.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Araponga.Tests.Bff.Journeys;

/// <summary>
/// Garante que cada jornada (path cacheável) usa o TTL configurado no cache.
/// </summary>
public sealed class JourneyCacheTtlTests
{
    private static IJourneyResponseCache CreateCacheWithJourneyTtls()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var options = new BffOptions
        {
            EnableCache = true,
            CacheTtlSeconds = 60,
            CacheTtlByPath = new Dictionary<string, int>
            {
                [BffJourneyRegistry.Onboarding] = 300,
                [BffJourneyRegistry.Feed] = 30,
                [BffJourneyRegistry.Events] = 45,
                [BffJourneyRegistry.Marketplace] = 60,
                [BffJourneyRegistry.Me] = 90,
                [BffJourneyRegistry.Connections] = 45,
                [BffJourneyRegistry.Territories] = 90,
                [BffJourneyRegistry.Membership] = 60,
                [BffJourneyRegistry.Map] = 90,
                [BffJourneyRegistry.Assets] = 60,
                [BffJourneyRegistry.Media] = 60,
                [BffJourneyRegistry.SubscriptionPlans] = 120,
                [BffJourneyRegistry.Subscriptions] = 120,
                [BffJourneyRegistry.Notifications] = 30,
                [BffJourneyRegistry.MarketplaceV1] = 60,
                [BffJourneyRegistry.Moderation] = 45,
                [BffJourneyRegistry.Chat] = 30,
                [BffJourneyRegistry.Alerts] = 45,
                [BffJourneyRegistry.Admin] = 30
            }
        };
        return new JourneyResponseCache(memoryCache, Options.Create(options));
    }

    [Theory]
    [InlineData("onboarding/suggested-territories", 300)]
    [InlineData("feed/territory-feed", 30)]
    [InlineData("events/territory-events", 45)]
    [InlineData("marketplace/search", 60)]
    [InlineData("me", 90)]
    [InlineData("me/profile", 90)]
    [InlineData("connections", 45)]
    [InlineData("connections/pending", 45)]
    [InlineData("territories", 90)]
    [InlineData("territories/paged", 90)]
    [InlineData("membership/me", 60)]
    [InlineData("map/entities", 90)]
    [InlineData("assets", 60)]
    [InlineData("media/00000000-0000-0000-0000-000000000001/info", 60)]
    [InlineData("subscription-plans", 120)]
    [InlineData("subscriptions/me", 120)]
    [InlineData("notifications/paged", 30)]
    [InlineData("marketplace-v1/cart", 60)]
    [InlineData("alerts", 45)]
    public void GetTtlSeconds_ForEachJourneyGetPath_ReturnsConfiguredTtl(string pathAndQuery, int expectedTtl)
    {
        var cache = CreateCacheWithJourneyTtls();
        Assert.Equal(expectedTtl, cache.GetTtlSeconds(pathAndQuery));
    }

    [Theory]
    [InlineData("onboarding/suggested-territories")]
    [InlineData("feed/territory-feed")]
    [InlineData("events/territory-events")]
    [InlineData("marketplace/search")]
    [InlineData("me")]
    [InlineData("me/profile")]
    [InlineData("connections")]
    [InlineData("connections/suggestions")]
    [InlineData("territories")]
    [InlineData("membership/me")]
    [InlineData("map/entities")]
    [InlineData("assets/paged")]
    [InlineData("media/00000000-0000-0000-0000-000000000001/info")]
    [InlineData("subscription-plans")]
    [InlineData("subscriptions/me")]
    [InlineData("notifications")]
    [InlineData("marketplace-v1/cart")]
    [InlineData("alerts/paged")]
    public void ShouldCache_ForEachJourneyGetPath_ReturnsTrue(string pathAndQuery)
    {
        var cache = CreateCacheWithJourneyTtls();
        Assert.True(cache.ShouldCache("GET", pathAndQuery, 200));
    }

    [Theory]
    [InlineData("auth")]
    [InlineData("auth/login")]
    [InlineData("auth/refresh")]
    public void ShouldCache_ForAuthPath_ReturnsFalse(string pathAndQuery)
    {
        var cache = CreateCacheWithJourneyTtls();
        Assert.False(cache.ShouldCache("GET", pathAndQuery, 200));
    }
}
