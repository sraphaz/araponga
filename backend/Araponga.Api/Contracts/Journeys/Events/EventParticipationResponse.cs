namespace Araponga.Api.Contracts.Journeys.Events;

public sealed record EventParticipationResponse(
    EventDetailJourneyDto Event,
    EventParticipantsSummaryDto Participants,
    UserEventParticipationDto UserParticipation);
