namespace Arah.Api.Services.Journeys.Backend;

public interface IOnboardingJourneyBackend
{
    Task<BackendTerritoryInfo?> GetTerritoryByIdAsync(Guid territoryId, CancellationToken cancellationToken = default);

    Task<bool> SetActiveTerritoryAsync(string sessionId, Guid territoryId, CancellationToken cancellationToken = default);

    Task<BackendMembershipInfo?> EnterAsVisitorAsync(Guid userId, Guid territoryId, CancellationToken cancellationToken = default);

    Task<BackendUserInfo?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<BackendPagedResult<BackendFeedPost>> ListFeedPagedAsync(
        Guid territoryId,
        Guid? userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, BackendPostCounts>> GetCountsByPostIdsAsync(
        IReadOnlyCollection<Guid> postIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BackendTerritorySuggestion>> GetTerritoriesNearbyAsync(
        double latitude,
        double longitude,
        double radiusKm,
        int limit,
        CancellationToken cancellationToken = default);
}

public sealed record BackendTerritorySuggestion(
    Guid Id,
    string Name,
    string? Description,
    double Latitude,
    double Longitude);
