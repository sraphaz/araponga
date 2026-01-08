namespace Araponga.Api.Contracts.Features;

public sealed record FeatureFlagResponse(
    Guid TerritoryId,
    IReadOnlyList<string> EnabledFlags
);
