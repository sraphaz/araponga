namespace Arah.Api.Contracts.Payout;

public sealed record SellerBalanceResponse(
    Guid Id,
    Guid TerritoryId,
    Guid SellerUserId,
    long PendingAmountInCents,
    long ReadyForPayoutAmountInCents,
    long PaidAmountInCents,
    string Currency,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
