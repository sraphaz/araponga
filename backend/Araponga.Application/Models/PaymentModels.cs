using Araponga.Application.Interfaces;

namespace Araponga.Application.Models;

/// <summary>
/// Modelo para criar um pagamento.
/// </summary>
public sealed record CreatePaymentRequest(
    Guid CheckoutId,
    string? ReturnUrl,
    Dictionary<string, string>? Metadata);

/// <summary>
/// Modelo de resposta de criação de pagamento.
/// </summary>
public sealed record CreatePaymentResponse(
    string PaymentIntentId,
    string? PaymentUrl,
    string? ClientSecret,
    Guid CheckoutId);

/// <summary>
/// Modelo para confirmar um pagamento.
/// </summary>
public sealed record ConfirmPaymentRequest(
    string PaymentIntentId);

/// <summary>
/// Modelo de resposta de confirmação de pagamento.
/// </summary>
public sealed record ConfirmPaymentResponse(
    string PaymentIntentId,
    PaymentStatus PaymentStatus,
    Guid CheckoutId);

/// <summary>
/// Modelo para criar um reembolso.
/// </summary>
public sealed record CreateRefundRequest(
    Guid CheckoutId,
    long? Amount,
    string? Reason);

/// <summary>
/// Modelo de resposta de reembolso.
/// </summary>
public sealed record CreateRefundResponse(
    string RefundId,
    long Amount,
    RefundStatus Status,
    Guid CheckoutId);
