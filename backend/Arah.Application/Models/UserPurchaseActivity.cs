namespace Arah.Application.Models;

public sealed record UserPurchaseActivity(
    Guid Id,
    Guid TerritoryId,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime CreatedAtUtc);
