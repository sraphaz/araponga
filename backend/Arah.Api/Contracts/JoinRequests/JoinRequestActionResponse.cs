namespace Arah.Api.Contracts.JoinRequests;

public sealed record JoinRequestActionResponse(
    Guid JoinRequestId,
    string Status,
    DateTime? DecidedAtUtc,
    Guid? DecidedByUserId);
