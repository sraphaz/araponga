using Araponga.Application.Common;
using Araponga.Application.Interfaces;

namespace Araponga.Infrastructure.Payments;

/// <summary>
/// Implementação mock do gateway de pagamento para desenvolvimento e testes.
/// Em produção, substitua por implementação real (Stripe, MercadoPago, etc.).
/// </summary>
public sealed class MockPaymentGateway : IPaymentGateway
{
    private readonly Dictionary<string, PaymentStatus> _paymentStatuses = new();
    private readonly Dictionary<string, RefundStatus> _refundStatuses = new();
    private int _paymentIntentCounter = 1;

    public Task<PaymentIntentResult> CreatePaymentIntentAsync(
        long amount,
        string currency,
        string description,
        Dictionary<string, string>? metadata,
        CancellationToken cancellationToken)
    {
        var paymentIntentId = $"pi_mock_{_paymentIntentCounter++}_{Guid.NewGuid():N}";
        var paymentUrl = $"https://mock-payment.example.com/pay/{paymentIntentId}";
        var clientSecret = $"mock_secret_{paymentIntentId}";

        // Inicializar como pendente
        _paymentStatuses[paymentIntentId] = PaymentStatus.Pending;

        return Task.FromResult(new PaymentIntentResult(
            paymentIntentId,
            paymentUrl,
            clientSecret));
    }

    public Task<PaymentStatusResult> GetPaymentStatusAsync(
        string paymentIntentId,
        CancellationToken cancellationToken)
    {
        if (!_paymentStatuses.TryGetValue(paymentIntentId, out var status))
        {
            return Task.FromResult(new PaymentStatusResult(
                PaymentStatus.Failed,
                "Payment intent not found",
                null));
        }

        var paidAt = status == PaymentStatus.Succeeded
            ? DateTime.UtcNow.AddMinutes(-5) // Simular pagamento há 5 minutos
            : (DateTime?)null;

        return Task.FromResult(new PaymentStatusResult(
            status,
            status == PaymentStatus.Failed ? "Mock payment failed" : null,
            paidAt));
    }

    public Task<PaymentWebhookEvent> ProcessWebhookAsync(
        string payload,
        string signature,
        CancellationToken cancellationToken)
    {
        // Em uma implementação real, validaria a assinatura e parsearia o payload
        // Por enquanto, vamos simular um evento de pagamento bem-sucedido
        // Em produção, você precisaria parsear o JSON do payload do gateway real

        // Para mock, vamos assumir que o payload contém payment_intent_id
        // Em produção, isso viria do webhook do gateway (Stripe, MercadoPago, etc.)
        var paymentIntentId = ExtractPaymentIntentIdFromPayload(payload);

        if (string.IsNullOrWhiteSpace(paymentIntentId))
        {
            throw new ArgumentException("Invalid webhook payload: payment_intent_id not found", nameof(payload));
        }

        // Simular que o pagamento foi bem-sucedido
        _paymentStatuses[paymentIntentId] = PaymentStatus.Succeeded;

        var metadata = new Dictionary<string, string>
        {
            { "source", "mock_gateway" }
        };

        return Task.FromResult(new PaymentWebhookEvent(
            "payment_intent.succeeded",
            paymentIntentId,
            PaymentStatus.Succeeded,
            metadata));
    }

    public Task<RefundResult> CreateRefundAsync(
        string paymentIntentId,
        long? amount,
        string? reason,
        CancellationToken cancellationToken)
    {
        if (!_paymentStatuses.TryGetValue(paymentIntentId, out var status) || status != PaymentStatus.Succeeded)
        {
            throw new InvalidOperationException("Cannot refund a payment that is not succeeded");
        }

        var refundId = $"refund_mock_{Guid.NewGuid():N}";
        var refundAmount = amount ?? 10000; // Valor padrão em centavos

        _refundStatuses[refundId] = RefundStatus.Succeeded;
        _paymentStatuses[paymentIntentId] = PaymentStatus.Refunded;

        return Task.FromResult(new RefundResult(
            refundId,
            refundAmount,
            RefundStatus.Succeeded,
            DateTime.UtcNow));
    }

    public Task<OperationResult> CancelPaymentIntentAsync(
        string paymentIntentId,
        CancellationToken cancellationToken)
    {
        if (!_paymentStatuses.TryGetValue(paymentIntentId, out var status))
        {
            return Task.FromResult(OperationResult.Failure("Payment intent not found"));
        }

        if (status == PaymentStatus.Succeeded)
        {
            return Task.FromResult(OperationResult.Failure("Cannot cancel a succeeded payment"));
        }

        _paymentStatuses[paymentIntentId] = PaymentStatus.Canceled;
        return Task.FromResult(OperationResult.Success());
    }

    /// <summary>
    /// Método auxiliar para extrair payment_intent_id do payload (mock).
    /// Em produção, isso seria parseado do JSON do gateway real.
    /// </summary>
    private static string? ExtractPaymentIntentIdFromPayload(string payload)
    {
        // Mock: assumir que o payload é JSON simples ou contém payment_intent_id
        // Em produção, você usaria System.Text.Json para parsear o payload do gateway
        if (payload.Contains("payment_intent_id"))
        {
            // Extração simples para mock
            var start = payload.IndexOf("payment_intent_id") + "payment_intent_id".Length + 1;
            var end = payload.IndexOfAny(new[] { '"', ',', '}' }, start);
            if (end > start)
            {
                return payload.Substring(start, end - start).Trim('"', ' ', ':');
            }
        }

        // Se não encontrar, tentar extrair qualquer ID que pareça payment intent
        if (payload.Contains("pi_"))
        {
            var start = payload.IndexOf("pi_");
            var end = payload.IndexOfAny(new[] { '"', ',', '}', ' ' }, start);
            if (end > start)
            {
                return payload.Substring(start, end - start).Trim('"', ' ', ':');
            }
        }

        return null;
    }

    /// <summary>
    /// Método auxiliar para testes: simular pagamento bem-sucedido.
    /// </summary>
    public void SimulatePaymentSuccess(string paymentIntentId)
    {
        _paymentStatuses[paymentIntentId] = PaymentStatus.Succeeded;
    }

    /// <summary>
    /// Método auxiliar para testes: simular falha de pagamento.
    /// </summary>
    public void SimulatePaymentFailure(string paymentIntentId)
    {
        _paymentStatuses[paymentIntentId] = PaymentStatus.Failed;
    }
}
