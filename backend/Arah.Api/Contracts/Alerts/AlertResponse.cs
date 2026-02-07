namespace Arah.Api.Contracts.Alerts;

public sealed record AlertResponse(
    Guid Id,
    string Title,
    string Description,
    string Status,
    DateTime CreatedAtUtc);
