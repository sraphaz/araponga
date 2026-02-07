namespace Arah.Api.Services.Journeys.Backend;

public interface IEventsJourneyBackend
{
    Task<BackendPagedResult<BackendEventDetail>> ListEventsPagedAsync(
        Guid territoryId,
        DateTime? from,
        DateTime? to,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BackendEventParticipationStatus>> GetUserParticipationsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, BackendEventDetail>> GetEventDetailsByIdsAsync(
        IReadOnlyCollection<Guid> eventIds,
        CancellationToken cancellationToken = default);

    Task<(string? CoverUrl, IReadOnlyList<string> AdditionalUrls)> GetEventMediaUrlsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    Task<BackendCreateEventResult> CreateEventAsync(
        Guid territoryId,
        Guid userId,
        string title,
        string? description,
        DateTime startsAtUtc,
        DateTime? endsAtUtc,
        double? latitude,
        double? longitude,
        string? locationLabel,
        Guid? coverMediaId,
        IReadOnlyList<Guid>? additionalMediaIds,
        CancellationToken cancellationToken = default);

    Task<(bool Success, string? Error)> SetParticipationAsync(
        Guid eventId,
        Guid userId,
        string status,
        CancellationToken cancellationToken = default);
}
