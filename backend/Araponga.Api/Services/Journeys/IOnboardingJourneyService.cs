using Araponga.Api.Contracts.Journeys.Onboarding;

namespace Araponga.Api.Services.Journeys;

public interface IOnboardingJourneyService
{
    Task<CompleteOnboardingResponse?> CompleteOnboardingAsync(
        Guid userId,
        string sessionId,
        Guid selectedTerritoryId,
        CancellationToken cancellationToken = default);

    Task<SuggestedTerritoriesResponse> GetSuggestedTerritoriesAsync(
        double latitude,
        double longitude,
        double radiusKm,
        CancellationToken cancellationToken = default);
}
