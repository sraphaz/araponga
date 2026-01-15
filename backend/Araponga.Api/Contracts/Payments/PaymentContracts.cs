namespace Araponga.Api.Contracts.Payments;

/// <summary>
/// Request para criar um pagamento.
/// </summary>
public sealed record CreatePaymentRequest(
    Guid CheckoutId,
    string? ReturnUrl,
    Dictionary<string, string>? Metadata);

/// <summary>
/// Response de criação de pagamento.
/// </summary>
public sealed record CreatePaymentResponse(
    string PaymentIntentId,
    string? PaymentUrl,
    string? ClientSecret,
    Guid CheckoutId);

/// <summary>
/// Request para confirmar um pagamento.
/// </summary>
public sealed record ConfirmPaymentRequest(
    string PaymentIntentId);

/// <summary>
/// Response de confirmação de pagamento.
/// </summary>
public sealed record ConfirmPaymentResponse(
    string PaymentIntentId,
    string PaymentStatus,
    Guid CheckoutId);

/// <summary>
/// Request para criar um reembolso.
/// </summary>
public sealed record CreateRefundRequest(
    Guid CheckoutId,
    long? Amount,
    string? Reason);

/// <summary>
/// Response de reembolso.
/// </summary>
public sealed record CreateRefundResponse(
    string RefundId,
    long Amount,
    string Status,
    Guid CheckoutId);
