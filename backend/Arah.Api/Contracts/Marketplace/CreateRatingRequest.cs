namespace Arah.Api.Contracts.Marketplace;

public sealed record CreateRatingRequest(
    int Rating,
    string? Comment);
