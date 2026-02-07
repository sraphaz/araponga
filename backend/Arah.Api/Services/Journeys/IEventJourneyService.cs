using Arah.Api.Contracts.Journeys.Events;

namespace Arah.Api.Services.Journeys;

public interface IEventJourneyService
{
    Task<TerritoryEventsJourneyResponse?> GetTerritoryEventsAsync(
        Guid territoryId,
        Guid? userId,
        DateTime? from,
        DateTime? to,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<CreateEventJourneyResponse?> CreateEventAsync(
        CreateEventJourneyRequest request,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<EventParticipationResponse?> ParticipateAsync(
        Guid eventId,
        Guid userId,
        string status,
        CancellationToken cancellationToken = default);
}
