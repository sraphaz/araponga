namespace Araponga.Bff.Contracts.Journeys.Feed;

public sealed record PostInteractionRequest(
    Guid PostId,
    Guid TerritoryId,
    string Action);
