using Araponga.Application.Common;

namespace Araponga.Application.Interfaces;

/// <summary>
/// Interface para abstração de gateways de pagamento.
/// Permite trocar facilmente entre diferentes provedores (Stripe, MercadoPago, PagSeguro, etc.).
/// </summary>
public interface IPaymentGateway
{
    /// <summary>
    /// Cria uma intenção de pagamento no gateway.
    /// </summary>
    /// <param name="amount">Valor do pagamento (em centavos ou menor unidade da moeda)</param>
    /// <param name="currency">Código da moeda (ex: "BRL", "USD")</param>
    /// <param name="description">Descrição do pagamento</param>
    /// <param name="metadata">Metadados adicionais (ex: checkoutId, userId)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>ID da intenção de pagamento no gateway e URL de pagamento (se aplicável)</returns>
    Task<PaymentIntentResult> CreatePaymentIntentAsync(
        long amount,
        string currency,
        string description,
        Dictionary<string, string>? metadata,
        CancellationToken cancellationToken);

    /// <summary>
    /// Confirma um pagamento após o usuário completar o fluxo no gateway.
    /// </summary>
    /// <param name="paymentIntentId">ID da intenção de pagamento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status do pagamento e detalhes</returns>
    Task<PaymentStatusResult> GetPaymentStatusAsync(
        string paymentIntentId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Processa um webhook do gateway e retorna informações do evento.
    /// </summary>
    /// <param name="payload">Payload do webhook (JSON string)</param>
    /// <param name="signature">Assinatura do webhook (para validação)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Evento de pagamento processado</returns>
    Task<PaymentWebhookEvent> ProcessWebhookAsync(
        string payload,
        string signature,
        CancellationToken cancellationToken);

    /// <summary>
    /// Cria um reembolso para um pagamento.
    /// </summary>
    /// <param name="paymentIntentId">ID da intenção de pagamento original</param>
    /// <param name="amount">Valor a reembolsar (em centavos, null = reembolso total)</param>
    /// <param name="reason">Motivo do reembolso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>ID do reembolso e status</returns>
    Task<RefundResult> CreateRefundAsync(
        string paymentIntentId,
        long? amount,
        string? reason,
        CancellationToken cancellationToken);

    /// <summary>
    /// Cancela uma intenção de pagamento que ainda não foi paga.
    /// </summary>
    /// <param name="paymentIntentId">ID da intenção de pagamento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<OperationResult> CancelPaymentIntentAsync(
        string paymentIntentId,
        CancellationToken cancellationToken);
}

/// <summary>
/// Resultado da criação de uma intenção de pagamento.
/// </summary>
public sealed record PaymentIntentResult(
    string PaymentIntentId,
    string? PaymentUrl,
    string? ClientSecret);

/// <summary>
/// Resultado da consulta de status de pagamento.
/// </summary>
public sealed record PaymentStatusResult(
    PaymentStatus Status,
    string? FailureReason,
    DateTime? PaidAtUtc);

/// <summary>
/// Status de um pagamento.
/// </summary>
public enum PaymentStatus
{
    Pending = 1,
    Processing = 2,
    Succeeded = 3,
    Failed = 4,
    Canceled = 5,
    Refunded = 6,
    PartiallyRefunded = 7
}

/// <summary>
/// Evento de webhook processado.
/// </summary>
public sealed record PaymentWebhookEvent(
    string EventType,
    string PaymentIntentId,
    PaymentStatus Status,
    Dictionary<string, string>? Metadata);

/// <summary>
/// Resultado de um reembolso.
/// </summary>
public sealed record RefundResult(
    string RefundId,
    long Amount,
    RefundStatus Status,
    DateTime CreatedAtUtc);

/// <summary>
/// Status de um reembolso.
/// </summary>
public enum RefundStatus
{
    Pending = 1,
    Succeeded = 2,
    Failed = 3,
    Canceled = 4
}
