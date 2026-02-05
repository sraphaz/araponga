using Araponga.Api.Contracts.Journeys.Common;

namespace Araponga.Api.Contracts.Journeys.Marketplace;

public sealed record MarketplaceSearchJourneyResponse(
    IReadOnlyList<MarketplaceItemJourneyDto> Items,
    IReadOnlyList<MarketplaceStoreSummaryDto> Stores,
    JourneyPaginationDto Pagination,
    MarketplaceFiltersDto? Filters,
    MarketplaceSuggestionsDto? Suggestions);

public sealed record MarketplaceItemJourneyDto(
    MarketplaceItemSummaryDto Item,
    MarketplaceStoreSummaryDto Store,
    MarketplaceItemMediaDto? Media,
    MarketplaceAvailabilityDto? Availability);

public sealed record MarketplaceItemSummaryDto(
    Guid Id,
    Guid TerritoryId,
    Guid StoreId,
    string Type,
    string Title,
    string? Description,
    string? Category,
    string PricingType,
    decimal? PriceAmount,
    string? Currency,
    string? Unit,
    string Status);

public sealed record MarketplaceStoreSummaryDto(
    Guid Id,
    Guid TerritoryId,
    string Name,
    string? Description,
    Guid OwnerUserId,
    string? OwnerDisplayName,
    string Status);

public sealed record MarketplaceItemMediaDto(
    string? PrimaryImageUrl,
    IReadOnlyList<string> ImageUrls);

public sealed record MarketplaceAvailabilityDto(bool InStock, int? Quantity);

public sealed record MarketplaceFiltersDto(
    IReadOnlyList<string> AvailableCategories,
    IReadOnlyList<string> AvailableTypes,
    PriceRangeDto? PriceRange);

public sealed record PriceRangeDto(decimal? Min, decimal? Max);

public sealed record MarketplaceSuggestionsDto(
    IReadOnlyList<MarketplaceItemSummaryDto>? TrendingItems,
    IReadOnlyList<MarketplaceItemSummaryDto>? RecommendedItems);
