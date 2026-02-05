using Araponga.Bff.Contracts.Journeys.Common;

namespace Araponga.Bff.Contracts.Journeys.Onboarding;

public sealed record TerritoryFeedInitialDto(
    IReadOnlyList<TerritoryFeedItemDto> Items,
    JourneyPaginationDto Pagination);
