namespace Araponga.Api.Contracts.Feed;

public sealed record FeedItemResponse(
    Guid Id,
    string Title,
    string Content,
    string Type,
    string Visibility,
    bool IsHighlighted,
    int LikeCount,
    int ShareCount,
    DateTime CreatedAtUtc
);
