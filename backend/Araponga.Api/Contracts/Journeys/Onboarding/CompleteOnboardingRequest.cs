namespace Araponga.Api.Contracts.Journeys.Onboarding;

public sealed record CompleteOnboardingRequest(
    string? AuthToken,
    Guid SelectedTerritoryId,
    LocationDto? Location);
