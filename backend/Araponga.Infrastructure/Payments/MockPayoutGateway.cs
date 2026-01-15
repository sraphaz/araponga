using Araponga.Application.Common;
using Araponga.Application.Interfaces;

namespace Araponga.Infrastructure.Payments;

/// <summary>
/// Implementação mock do gateway de payout para desenvolvimento e testes.
/// Em produção, substitua por implementação real (Stripe Connect, MercadoPago, etc.).
/// </summary>
public sealed class MockPayoutGateway : IPayoutGateway
{
    private readonly Dictionary<string, PayoutStatus> _payoutStatuses = new();
    private int _payoutCounter = 1;

    public Task<OperationResult<PayoutResult>> CreatePayoutAsync(
        long amountInCents,
        string currency,
        string sellerAccountId,
        string description,
        Dictionary<string, string>? metadata,
        CancellationToken cancellationToken)
    {
        var payoutId = $"po_mock_{_payoutCounter++}_{Guid.NewGuid():N}";

        // Inicializar como pendente
        _payoutStatuses[payoutId] = PayoutStatus.Pending;

        var result = new PayoutResult(
            payoutId,
            PayoutStatus.Pending,
            DateTime.UtcNow);

        return Task.FromResult(OperationResult<PayoutResult>.Success(result));
    }

    public Task<OperationResult<PayoutStatusResult>> GetPayoutStatusAsync(
        string payoutId,
        CancellationToken cancellationToken)
    {
        if (!_payoutStatuses.TryGetValue(payoutId, out var status))
        {
            return Task.FromResult(OperationResult<PayoutStatusResult>.Failure(
                "Payout not found"));
        }

        var completedAt = status == PayoutStatus.Completed
            ? DateTime.UtcNow.AddMinutes(-5) // Simular payout há 5 minutos
            : (DateTime?)null;

        var result = new PayoutStatusResult(
            payoutId,
            status,
            status == PayoutStatus.Failed ? "Mock payout failed" : null,
            completedAt);

        return Task.FromResult(OperationResult<PayoutStatusResult>.Success(result));
    }

    public Task<OperationResult> CancelPayoutAsync(
        string payoutId,
        CancellationToken cancellationToken)
    {
        if (!_payoutStatuses.TryGetValue(payoutId, out var status))
        {
            return Task.FromResult(OperationResult.Failure("Payout not found"));
        }

        if (status == PayoutStatus.Completed)
        {
            return Task.FromResult(OperationResult.Failure("Cannot cancel a completed payout"));
        }

        _payoutStatuses[payoutId] = PayoutStatus.Canceled;
        return Task.FromResult(OperationResult.Success());
    }

    /// <summary>
    /// Método auxiliar para testes: simular payout bem-sucedido.
    /// </summary>
    public void SimulatePayoutSuccess(string payoutId)
    {
        _payoutStatuses[payoutId] = PayoutStatus.Completed;
    }

    /// <summary>
    /// Método auxiliar para testes: simular falha de payout.
    /// </summary>
    public void SimulatePayoutFailure(string payoutId)
    {
        _payoutStatuses[payoutId] = PayoutStatus.Failed;
    }
}
