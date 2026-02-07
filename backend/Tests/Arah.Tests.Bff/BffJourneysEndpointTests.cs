using System.Net;
using System.Net.Http.Json;
using Arah.Bff;
using Arah.Bff.Journeys;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Arah.Tests.Bff;

/// <summary>
/// Testes de integração do endpoint GET /bff/journeys que lista todas as jornadas expostas pelo BFF.
/// </summary>
public sealed class BffJourneysEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BffJourneysEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetBffJourneys_ReturnsOk()
    {
        var response = await _client.GetAsync("/bff/journeys");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetBffJourneys_ReturnsAllTwentyJourneys()
    {
        var response = await _client.GetAsync("/bff/journeys");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<BffJourneysResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json.Journeys);
        Assert.Equal(BffJourneyRegistry.AllEndpoints.Count, json.Journeys.Count);
        var names = json.Journeys.Select(j => j.Journey).ToHashSet();
        foreach (var journeyName in BffJourneyRegistry.AllPathPrefixes)
            Assert.Contains(journeyName, names);
    }

    [Fact]
    public async Task GetBffJourneys_EachJourneyHasBasePathAndAllEndpoints()
    {
        var response = await _client.GetAsync("/bff/journeys");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<BffJourneysResponse>();
        Assert.NotNull(json);
        var expectedCounts = BffJourneyRegistry.AllEndpoints.ToDictionary(
            kv => kv.Key, kv => kv.Value.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var j in json.Journeys!)
        {
            Assert.NotNull(j.BasePath);
            Assert.StartsWith(BffJourneyRegistry.BasePath, j.BasePath);
            Assert.NotNull(j.Endpoints);
            Assert.True(expectedCounts.TryGetValue(j.Journey!, out var expected), $"Journey {j.Journey} should be known.");
            Assert.Equal(expected, j.Endpoints.Count);
        }
    }

    private sealed class BffJourneysResponse
    {
        public List<JourneyItem>? Journeys { get; set; }
    }

    private sealed class JourneyItem
    {
        public string? Journey { get; set; }
        public string? BasePath { get; set; }
        public List<EndpointItem>? Endpoints { get; set; }
    }

    private sealed class EndpointItem
    {
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? Description { get; set; }
    }
}
