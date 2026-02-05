namespace Araponga.Api.Contracts.Journeys.Onboarding;

public sealed record UserSummary(
    Guid Id,
    string DisplayName,
    string? Email,
    string Membership);
