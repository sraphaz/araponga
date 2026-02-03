namespace Araponga.Bff.Contracts.Journeys.Events;

public sealed record CreateEventJourneyResponse(
    EventDetailJourneyDto Event,
    IReadOnlyList<string> MediaUrls);
