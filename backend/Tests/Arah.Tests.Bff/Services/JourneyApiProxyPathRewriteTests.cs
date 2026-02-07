using Arah.Bff.Services;
using Xunit;

namespace Arah.Tests.Bff.Services;

/// <summary>
/// Garante que o proxy reescreve path das jornadas v1 (auth, me) para api/v1/* e mantém v2 em api/v2/journeys/*.
/// </summary>
public sealed class JourneyApiProxyPathRewriteTests
{
    private const string BaseUrl = "https://api.example.com";

    [Fact]
    public void BuildForwardUri_Onboarding_RequestsApiV2Journeys()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "onboarding/suggested-territories", "latitude=1&longitude=2");
        Assert.Equal("https://api.example.com/api/v2/journeys/onboarding/suggested-territories?latitude=1&longitude=2", uri);
    }

    [Fact]
    public void BuildForwardUri_AuthLogin_RequestsApiV1Auth()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "auth/login");
        Assert.Equal("https://api.example.com/api/v1/auth/login", uri);
    }

    [Fact]
    public void BuildForwardUri_MeProfile_RequestsApiV1UsersMe()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "me/profile");
        Assert.Equal("https://api.example.com/api/v1/users/me/profile", uri);
    }

    [Fact]
    public void BuildForwardUri_MeOnly_RequestsApiV1UsersMe()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "me");
        Assert.Equal("https://api.example.com/api/v1/users/me", uri);
    }

    [Fact]
    public void BuildForwardUri_FeedTerritoryFeed_RequestsApiV2Journeys()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "feed/territory-feed");
        Assert.Equal("https://api.example.com/api/v2/journeys/feed/territory-feed", uri);
    }

    [Fact]
    public void BuildForwardUri_WithLeadingQuestionMark_KeepsQueryString()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "me/profile", "?foo=bar");
        Assert.Equal("https://api.example.com/api/v1/users/me/profile?foo=bar", uri);
    }

    [Fact]
    public void BuildForwardUri_Connections_RequestsApiV1Connections()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "connections");
        Assert.Equal("https://api.example.com/api/v1/connections", uri);
    }

    [Fact]
    public void BuildForwardUri_ConnectionsPending_RequestsApiV1ConnectionsPending()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "connections/pending");
        Assert.Equal("https://api.example.com/api/v1/connections/pending", uri);
    }

    [Fact]
    public void BuildForwardUri_Territories_RequestsApiV1Territories()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "territories");
        Assert.Equal("https://api.example.com/api/v1/territories", uri);
    }

    [Fact]
    public void BuildForwardUri_TerritoriesIdEnter_RequestsApiV1TerritoriesIdEnter()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "territories/abc-123-guid/enter");
        Assert.Equal("https://api.example.com/api/v1/territories/abc-123-guid/enter", uri);
    }

    [Fact]
    public void BuildForwardUri_MembershipMe_RequestsApiV1MembershipsMe()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "membership/me");
        Assert.Equal("https://api.example.com/api/v1/memberships/me", uri);
    }

    [Fact]
    public void BuildForwardUri_MapEntities_RequestsApiV1Map()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "map/entities");
        Assert.Equal("https://api.example.com/api/v1/map/entities", uri);
    }

    [Fact]
    public void BuildForwardUri_Assets_RequestsApiV1Assets()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "assets");
        Assert.Equal("https://api.example.com/api/v1/assets", uri);
    }

    [Fact]
    public void BuildForwardUri_MediaIdInfo_RequestsApiV1Media()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "media/abc-123/info");
        Assert.Equal("https://api.example.com/api/v1/media/abc-123/info", uri);
    }

    [Fact]
    public void BuildForwardUri_SubscriptionPlans_RequestsApiV1SubscriptionPlans()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "subscription-plans");
        Assert.Equal("https://api.example.com/api/v1/subscription-plans", uri);
    }

    [Fact]
    public void BuildForwardUri_SubscriptionsMe_RequestsApiV1SubscriptionsMe()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "subscriptions/me");
        Assert.Equal("https://api.example.com/api/v1/subscriptions/me", uri);
    }

    [Fact]
    public void BuildForwardUri_Notifications_RequestsApiV1Notifications()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "notifications");
        Assert.Equal("https://api.example.com/api/v1/notifications", uri);
    }

    [Fact]
    public void BuildForwardUri_MarketplaceV1Cart_RequestsApiV1Cart()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "marketplace-v1/cart");
        Assert.Equal("https://api.example.com/api/v1/cart", uri);
    }

    [Fact]
    public void BuildForwardUri_AdminSeed_RequestsApiV1AdminSeed()
    {
        var uri = JourneyApiProxy.BuildForwardUri(BaseUrl, "admin/seed");
        Assert.Equal("https://api.example.com/api/v1/admin/seed", uri);
    }
}
