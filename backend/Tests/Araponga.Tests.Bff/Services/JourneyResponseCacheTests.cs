using Araponga.Bff.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;

namespace Araponga.Tests.Bff.Services;

public sealed class JourneyResponseCacheTests
{
    private static IJourneyResponseCache CreateCache(BffOptions options)
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var opts = Options.Create(options);
        return new JourneyResponseCache(memoryCache, opts);
    }

    [Fact]
    public void ShouldCache_WhenCacheDisabled_ReturnsFalse()
    {
        var cache = CreateCache(new BffOptions { EnableCache = false });
        Assert.False(cache.ShouldCache("GET", "feed/territory-feed", 200));
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    public void ShouldCache_WhenNotGet_ReturnsFalse(string method)
    {
        var cache = CreateCache(new BffOptions { EnableCache = true });
        Assert.False(cache.ShouldCache(method, "feed/territory-feed", 200));
    }

    [Theory]
    [InlineData(199)]
    [InlineData(300)]
    [InlineData(404)]
    [InlineData(500)]
    public void ShouldCache_WhenStatusNot2xx_ReturnsFalse(int statusCode)
    {
        var cache = CreateCache(new BffOptions { EnableCache = true });
        Assert.False(cache.ShouldCache("GET", "feed/territory-feed", statusCode));
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(204)]
    [InlineData(299)]
    public void ShouldCache_WhenGetAnd2xx_ReturnsTrue(int statusCode)
    {
        var cache = CreateCache(new BffOptions { EnableCache = true });
        Assert.True(cache.ShouldCache("GET", "feed/territory-feed", statusCode));
    }

    [Theory]
    [InlineData("auth")]
    [InlineData("auth/login")]
    [InlineData("auth/refresh")]
    public void ShouldCache_WhenPathIsAuth_ReturnsFalse(string pathAndQuery)
    {
        var cache = CreateCache(new BffOptions { EnableCache = true });
        Assert.False(cache.ShouldCache("GET", pathAndQuery, 200));
    }

    [Fact]
    public void GetTtlSeconds_WhenNoPathConfig_ReturnsDefault()
    {
        var cache = CreateCache(new BffOptions { CacheTtlSeconds = 120 });
        Assert.Equal(120, cache.GetTtlSeconds("feed/territory-feed"));
    }

    [Fact]
    public void GetTtlSeconds_WhenPathConfigMatches_ReturnsPathTtl()
    {
        var cache = CreateCache(new BffOptions
        {
            CacheTtlSeconds = 60,
            CacheTtlByPath = new Dictionary<string, int> { ["feed"] = 30, ["onboarding"] = 300 }
        });
        Assert.Equal(30, cache.GetTtlSeconds("feed/territory-feed"));
        Assert.Equal(300, cache.GetTtlSeconds("onboarding/suggested-territories"));
    }

    [Fact]
    public void GetTtlSeconds_WhenLongerPrefixMatches_ReturnsLongerPrefixTtl()
    {
        var cache = CreateCache(new BffOptions
        {
            CacheTtlSeconds = 60,
            CacheTtlByPath = new Dictionary<string, int> { ["feed"] = 30, ["feed/territory"] = 90 }
        });
        Assert.Equal(90, cache.GetTtlSeconds("feed/territory-feed"));
    }

    [Fact]
    public void TryGet_Set_ThenTryGet_ReturnsCachedResponse()
    {
        var cache = CreateCache(new BffOptions { EnableCache = true });
        var response = new CachedJourneyResponse(
            200,
            new Dictionary<string, string[]> { ["Content-Type"] = new[] { "application/json" } },
            "application/json",
            "[]"u8.ToArray());

        cache.Set("key1", response, TimeSpan.FromSeconds(60));

        Assert.True(cache.TryGet("key1", out var cached));
        Assert.NotNull(cached);
        Assert.Equal(200, cached.StatusCode);
        Assert.Equal("application/json", cached.Headers["Content-Type"][0]);
        Assert.Equal("[]"u8.ToArray(), cached.Body);
    }

    [Fact]
    public void TryGet_WhenKeyNotSet_ReturnsFalse()
    {
        var cache = CreateCache(new BffOptions { EnableCache = true });
        Assert.False(cache.TryGet("nonexistent", out var cached));
        Assert.Null(cached);
    }
}
