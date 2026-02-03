namespace Araponga.Api.Contracts.Journeys.Events;

public sealed record EventParticipationRequest(
    Guid EventId,
    string Status); // "INTERESTED" | "CONFIRMED"
