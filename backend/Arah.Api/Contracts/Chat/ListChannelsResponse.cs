namespace Arah.Api.Contracts.Chat;

public sealed record ListChannelsResponse(
    Guid TerritoryId,
    IReadOnlyList<ConversationSummaryResponse> Channels);

