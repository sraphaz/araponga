namespace Araponga.Api.Contracts.Memberships;

public sealed record MembershipStatusResponse(
    Guid UserId,
    Guid TerritoryId,
    string Role,
    string VerificationStatus
);
