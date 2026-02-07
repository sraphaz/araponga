namespace Arah.Api.Contracts.JoinRequests;

public sealed record JoinRequestCreatedResponse(
    Guid JoinRequestId,
    string Status,
    DateTime CreatedAtUtc);
