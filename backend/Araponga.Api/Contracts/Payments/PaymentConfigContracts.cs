namespace Araponga.Api.Contracts.Payments;

/// <summary>
/// Response de configuração de pagamento do território.
/// </summary>
public sealed record TerritoryPaymentConfigResponse(
    Guid Id,
    Guid TerritoryId,
    string GatewayProvider,
    bool IsActive,
    string Currency,
    long MinimumAmount,
    long? MaximumAmount,
    bool ShowFeeBreakdown,
    string FeeTransparencyLevel);

/// <summary>
/// Request para criar ou atualizar configuração de pagamento.
/// </summary>
public sealed record UpsertPaymentConfigRequest(
    string GatewayProvider,
    bool IsActive,
    string Currency,
    long MinimumAmount,
    long? MaximumAmount,
    bool ShowFeeBreakdown,
    string FeeTransparencyLevel);

/// <summary>
/// Request para calcular breakdown de fees.
/// </summary>
public sealed record CalculateFeesRequest(
    long AmountInCents,
    string ItemType);

/// <summary>
/// Response de breakdown de fees.
/// </summary>
public sealed record FeeBreakdownResponse(
    long SubtotalInCents,
    long PlatformFeeInCents,
    long TotalInCents,
    string Currency,
    decimal PlatformFeePercent,
    decimal PlatformFeeFixed,
    bool ShowBreakdown,
    string TransparencyLevel);
