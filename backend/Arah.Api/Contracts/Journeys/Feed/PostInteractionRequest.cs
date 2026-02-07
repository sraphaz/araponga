namespace Arah.Api.Contracts.Journeys.Feed;

public sealed record PostInteractionRequest(
    Guid PostId,
    Guid TerritoryId,
    string Action); // "LIKE" | "COMMENT" | "SHARE"
