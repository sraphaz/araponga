namespace Arah.Api.Contracts.JoinRequests;

public sealed record CreateJoinRequestRequest(
    IReadOnlyCollection<Guid> RecipientUserIds,
    string? Message);
