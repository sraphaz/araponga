namespace Araponga.Api.Contracts.Journeys.Onboarding;

public sealed record SuggestedAction(
    string Action,
    string Title,
    string? Description,
    string Priority);
