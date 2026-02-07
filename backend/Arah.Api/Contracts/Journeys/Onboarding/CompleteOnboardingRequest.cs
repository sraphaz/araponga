namespace Arah.Api.Contracts.Journeys.Onboarding;

public sealed record CompleteOnboardingRequest(
    string? AuthToken,
    Guid SelectedTerritoryId,
    LocationDto? Location);
