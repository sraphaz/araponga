namespace Araponga.Api.Contracts.Health;

public sealed record HealthIndicatorResponse(
    string Name,
    string Status,
    string Description
);
