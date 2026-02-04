namespace Araponga.Api.Contracts.Journeys.Feed;

public sealed record PostInteractionResponse(
    TerritoryFeedPostDto Post,
    TerritoryFeedCountsDto Counts,
    UserInteractionsDto UserInteractions);
