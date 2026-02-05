using Araponga.Api.Contracts.Journeys.Common;

namespace Araponga.Api.Contracts.Journeys.Onboarding;

/// <summary>
/// Feed inicial resumido para resposta do onboarding (itens + paginação).
/// </summary>
public sealed record TerritoryFeedInitialDto(
    IReadOnlyList<TerritoryFeedItemDto> Items,
    JourneyPaginationDto Pagination);
