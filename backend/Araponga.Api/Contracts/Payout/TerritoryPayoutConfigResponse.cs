namespace Araponga.Api.Contracts.Payout;

public sealed record TerritoryPayoutConfigResponse(
    Guid Id,
    Guid TerritoryId,
    int RetentionPeriodDays,
    long MinimumPayoutAmountInCents,
    long? MaximumPayoutAmountInCents,
    string Frequency,
    bool AutoPayoutEnabled,
    bool RequiresApproval,
    string Currency,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
