namespace Arah.Api.Contracts.Memberships;

public sealed record MembershipResponse(
    Guid Id,
    Guid UserId,
    Guid TerritoryId,
    string Role,
    string VerificationStatus,
    DateTime CreatedAtUtc
);
