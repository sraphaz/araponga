using Araponga.Api.Contracts.Common;

namespace Araponga.Api.Contracts.Journeys.Onboarding;

public sealed record CompleteOnboardingResponse(
    UserSummary User,
    TerritorySummary Territory,
    TerritoryFeedInitialDto InitialFeed,
    IReadOnlyList<SuggestedAction> SuggestedActions);
