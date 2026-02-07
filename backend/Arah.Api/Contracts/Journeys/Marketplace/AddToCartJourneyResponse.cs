namespace Arah.Api.Contracts.Journeys.Marketplace;

public sealed record AddToCartJourneyResponse(
    bool Success,
    CartJourneyDto Cart,
    MarketplaceItemJourneyDto? Item,
    CartTotalDto? Total,
    CartSuggestionsDto? Suggestions);

public sealed record CartJourneyDto(
    Guid CartId,
    Guid TerritoryId,
    IReadOnlyList<CartItemJourneyDto> Items,
    CartTotalDto? Total);

public sealed record CartItemJourneyDto(
    Guid Id,
    MarketplaceItemSummaryDto Item,
    MarketplaceStoreSummaryDto Store,
    int Quantity,
    string? Notes,
    bool IsPurchasable);

public sealed record CartTotalDto(
    decimal Subtotal,
    decimal PlatformFee,
    decimal Total,
    string? Currency);

public sealed record CartSuggestionsDto(
    IReadOnlyList<MarketplaceItemSummaryDto>? FrequentlyBoughtTogether,
    IReadOnlyList<MarketplaceItemSummaryDto>? SimilarItems);
