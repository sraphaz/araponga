namespace Arah.Api.Contracts.Payout;

public sealed record SellerTransactionResponse(
    Guid Id,
    Guid TerritoryId,
    Guid StoreId,
    Guid CheckoutId,
    long GrossAmountInCents,
    long PlatformFeeInCents,
    long NetAmountInCents,
    string Currency,
    string Status,
    string? PayoutId,
    DateTime CreatedAtUtc,
    DateTime? ReadyForPayoutAtUtc,
    DateTime? PaidAtUtc,
    DateTime UpdatedAtUtc);
