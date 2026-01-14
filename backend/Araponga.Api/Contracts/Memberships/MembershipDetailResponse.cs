namespace Araponga.Api.Contracts.Memberships;

public sealed record MembershipDetailResponse(
    Guid Id,
    Guid UserId,
    Guid TerritoryId,
    string Role,
    string ResidencyVerification,
    DateTime? LastGeoVerifiedAtUtc,
    DateTime? LastDocumentVerifiedAtUtc,
    DateTime CreatedAtUtc
);

public sealed record MyMembershipsResponse(
    IReadOnlyList<MembershipDetailResponse> Memberships
);
