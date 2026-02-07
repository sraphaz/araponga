namespace Arah.Api.Contracts.Journeys.Events;

public sealed record CreateEventJourneyResponse(
    EventDetailJourneyDto Event,
    IReadOnlyList<string> MediaUrls);
