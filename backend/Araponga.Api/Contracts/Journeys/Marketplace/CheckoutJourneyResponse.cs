namespace Araponga.Api.Contracts.Journeys.Marketplace;

public sealed record CheckoutJourneyResponse(
    bool Success,
    IReadOnlyList<OrderSummaryDto> Orders,
    IReadOnlyList<CheckoutSummaryDto> Summaries);

public sealed record OrderSummaryDto(
    Guid Id,
    Guid TerritoryId,
    Guid StoreId,
    string Status,
    CartTotalDto? Total,
    IReadOnlyList<CheckoutItemSummaryDto> Items,
    DateTime CreatedAtUtc);

public sealed record CheckoutItemSummaryDto(
    Guid Id,
    Guid ItemId,
    string TitleSnapshot,
    int Quantity,
    decimal? UnitPrice,
    decimal? LineTotal);

public sealed record CheckoutSummaryDto(
    Guid StoreId,
    int CheckoutItemCount,
    int InquiryCount);
