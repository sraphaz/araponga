namespace Arah.Api.Contracts.Journeys.Marketplace;

public sealed record AddToCartJourneyRequest(
    Guid TerritoryId,
    Guid ItemId,
    int Quantity,
    string? Notes);
