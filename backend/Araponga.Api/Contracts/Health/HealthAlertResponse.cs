namespace Araponga.Api.Contracts.Health;

public sealed record HealthAlertResponse(
    Guid Id,
    string Title,
    string Description,
    string Status,
    DateTime CreatedAtUtc
);
