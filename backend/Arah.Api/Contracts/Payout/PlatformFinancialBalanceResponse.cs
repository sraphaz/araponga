namespace Arah.Api.Contracts.Payout;

public sealed record PlatformFinancialBalanceResponse(
    Guid Id,
    Guid TerritoryId,
    long TotalRevenueInCents,
    long TotalExpensesInCents,
    long NetBalanceInCents,
    string Currency,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
