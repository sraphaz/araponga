namespace Arah.Api.Contracts.Payout;

public sealed record PlatformExpenseTransactionResponse(
    Guid Id,
    Guid TerritoryId,
    Guid SellerTransactionId,
    long PayoutAmountInCents,
    string Currency,
    string? PayoutId,
    Guid? FinancialTransactionId,
    DateTime CreatedAtUtc);
