namespace Arah.Api.Contracts.JoinRequests;

public sealed record IncomingJoinRequestResponse(
    Guid JoinRequestId,
    Guid TerritoryId,
    Guid RequesterUserId,
    string RequesterDisplayName,
    string? Message,
    DateTime CreatedAtUtc);
