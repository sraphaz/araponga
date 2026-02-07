namespace Arah.Api.Contracts.Features;

public sealed record UpdateFeatureFlagsRequest(
    IReadOnlyList<string> EnabledFlags
);
