namespace Arah.Api.Contracts.Chat;

public sealed record ListGroupsResponse(
    Guid TerritoryId,
    IReadOnlyList<ConversationSummaryResponse> Groups);

