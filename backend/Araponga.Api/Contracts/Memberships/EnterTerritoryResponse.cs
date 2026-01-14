namespace Araponga.Api.Contracts.Memberships;

public sealed record EnterTerritoryResponse(
    Guid Id,
    Guid UserId,
    Guid TerritoryId,
    string Role,
    string ResidencyVerification,
    DateTime? LastGeoVerifiedAtUtc,
    DateTime? LastDocumentVerifiedAtUtc,
    DateTime CreatedAtUtc
);
