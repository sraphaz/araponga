namespace Araponga.Api.Contracts.Payout;

public sealed record PlatformRevenueTransactionResponse(
    Guid Id,
    Guid TerritoryId,
    Guid CheckoutId,
    long FeeAmountInCents,
    string Currency,
    Guid? FinancialTransactionId,
    DateTime CreatedAtUtc);
