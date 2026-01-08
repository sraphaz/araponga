namespace Araponga.Api.Contracts.Map;

public sealed record MapEntityResponse(
    Guid Id,
    string Name,
    string Category,
    string Status,
    string Visibility,
    int ConfirmationCount,
    DateTime CreatedAtUtc
);
