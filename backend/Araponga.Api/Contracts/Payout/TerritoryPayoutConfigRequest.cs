namespace Araponga.Api.Contracts.Payout;

public sealed record TerritoryPayoutConfigRequest(
    int RetentionPeriodDays,
    long MinimumPayoutAmountInCents,
    long? MaximumPayoutAmountInCents,
    string Frequency, // "Daily", "Weekly", "Monthly", "Manual"
    bool AutoPayoutEnabled,
    bool RequiresApproval,
    string Currency);
