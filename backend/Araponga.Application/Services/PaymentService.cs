using Araponga.Application.Common;
using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Araponga.Domain.Marketplace;

namespace Araponga.Application.Services;

/// <summary>
/// Serviço para gerenciar pagamentos de checkouts.
/// </summary>
public sealed class PaymentService
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ICheckoutRepository _checkoutRepository;
    private readonly TerritoryPaymentConfigService _paymentConfigService;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(
        IPaymentGateway paymentGateway,
        ICheckoutRepository checkoutRepository,
        TerritoryPaymentConfigService paymentConfigService,
        IUnitOfWork unitOfWork)
    {
        _paymentGateway = paymentGateway;
        _checkoutRepository = checkoutRepository;
        _paymentConfigService = paymentConfigService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Cria um pagamento para um checkout.
    /// </summary>
    public async Task<Result<CreatePaymentResponse>> CreatePaymentAsync(
        Guid checkoutId,
        Guid userId,
        string? returnUrl,
        Dictionary<string, string>? metadata,
        CancellationToken cancellationToken)
    {
        var checkout = await GetCheckoutByIdAsync(checkoutId, cancellationToken);
        if (checkout is null)
        {
            return Result<CreatePaymentResponse>.Failure("Checkout not found.");
        }

        if (checkout.BuyerUserId != userId)
        {
            return Result<CreatePaymentResponse>.Failure("User is not authorized to pay for this checkout.");
        }

        if (checkout.Status != CheckoutStatus.Created && checkout.Status != CheckoutStatus.AwaitingPayment)
        {
            return Result<CreatePaymentResponse>.Failure($"Checkout cannot be paid. Current status: {checkout.Status}");
        }

        if (checkout.TotalAmount is null || checkout.TotalAmount <= 0)
        {
            return Result<CreatePaymentResponse>.Failure("Checkout total amount is invalid.");
        }

        // Verificar se pagamentos estão habilitados para o território
        var isPaymentEnabled = await _paymentConfigService.IsPaymentEnabledAsync(checkout.TerritoryId, cancellationToken);
        if (!isPaymentEnabled)
        {
            return Result<CreatePaymentResponse>.Failure(
                "Payments are not enabled for this territory. Please contact the territory administrator.");
        }

        // Converter decimal para centavos
        var amountInCents = (long)(checkout.TotalAmount.Value * 100);

        // Validar valor contra limites configurados
        var amountValidation = await _paymentConfigService.ValidateAmountAsync(
            checkout.TerritoryId,
            amountInCents,
            cancellationToken);
        if (amountValidation.IsFailure)
        {
            return Result<CreatePaymentResponse>.Failure(amountValidation.Error ?? "Amount validation failed.");
        }

        var paymentMetadata = new Dictionary<string, string>
        {
            { "checkoutId", checkoutId.ToString() },
            { "userId", userId.ToString() },
            { "territoryId", checkout.TerritoryId.ToString() },
            { "storeId", checkout.StoreId.ToString() }
        };

        if (metadata is not null)
        {
            foreach (var (key, value) in metadata)
            {
                paymentMetadata[key] = value;
            }
        }

        var description = $"Checkout #{checkoutId} - Store {checkout.StoreId}";

        var paymentIntentResult = await _paymentGateway.CreatePaymentIntentAsync(
            amountInCents,
            checkout.Currency,
            description,
            paymentMetadata,
            cancellationToken);

        // Atualizar checkout com PaymentIntentId e status
        checkout.SetPaymentIntentId(paymentIntentResult.PaymentIntentId, DateTime.UtcNow);
        checkout.SetStatus(CheckoutStatus.AwaitingPayment, DateTime.UtcNow);

        await _checkoutRepository.UpdateAsync(checkout, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<CreatePaymentResponse>.Success(new CreatePaymentResponse(
            paymentIntentResult.PaymentIntentId,
            paymentIntentResult.PaymentUrl,
            paymentIntentResult.ClientSecret,
            checkoutId));
    }

    /// <summary>
    /// Confirma um pagamento após o usuário completar o fluxo no gateway.
    /// </summary>
    public async Task<Result<ConfirmPaymentResponse>> ConfirmPaymentAsync(
        string paymentIntentId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var checkout = await GetCheckoutByPaymentIntentIdAsync(paymentIntentId, cancellationToken);
        if (checkout is null)
        {
            return Result<ConfirmPaymentResponse>.Failure("Checkout not found for this payment intent.");
        }

        if (checkout.BuyerUserId != userId)
        {
            return Result<ConfirmPaymentResponse>.Failure("User is not authorized to confirm this payment.");
        }

        var statusResult = await _paymentGateway.GetPaymentStatusAsync(paymentIntentId, cancellationToken);

        var now = DateTime.UtcNow;

        switch (statusResult.Status)
        {
            case PaymentStatus.Succeeded:
                checkout.SetStatus(CheckoutStatus.Paid, now);
                break;
            case PaymentStatus.Failed:
                checkout.SetStatus(CheckoutStatus.Canceled, now);
                break;
            case PaymentStatus.Canceled:
                checkout.SetStatus(CheckoutStatus.Canceled, now);
                break;
            case PaymentStatus.Pending:
            case PaymentStatus.Processing:
                // Manter status atual
                break;
            default:
                return Result<ConfirmPaymentResponse>.Failure($"Unexpected payment status: {statusResult.Status}");
        }

        await _checkoutRepository.UpdateAsync(checkout, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<ConfirmPaymentResponse>.Success(new ConfirmPaymentResponse(
            paymentIntentId,
            statusResult.Status,
            checkout.Id));
    }

    /// <summary>
    /// Processa um webhook do gateway de pagamento.
    /// </summary>
    public async Task<OperationResult> ProcessWebhookAsync(
        string payload,
        string signature,
        CancellationToken cancellationToken)
    {
        var webhookEvent = await _paymentGateway.ProcessWebhookAsync(payload, signature, cancellationToken);

        var checkout = await GetCheckoutByPaymentIntentIdAsync(webhookEvent.PaymentIntentId, cancellationToken);
        if (checkout is null)
        {
            return OperationResult.Failure("Checkout not found for this payment intent.");
        }

        var now = DateTime.UtcNow;

        switch (webhookEvent.Status)
        {
            case PaymentStatus.Succeeded:
                checkout.SetStatus(CheckoutStatus.Paid, now);
                break;
            case PaymentStatus.Failed:
                checkout.SetStatus(CheckoutStatus.Canceled, now);
                break;
            case PaymentStatus.Canceled:
                checkout.SetStatus(CheckoutStatus.Canceled, now);
                break;
            case PaymentStatus.Refunded:
            case PaymentStatus.PartiallyRefunded:
                // Status de reembolso pode ser tratado separadamente se necessário
                break;
        }

        await _checkoutRepository.UpdateAsync(checkout, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return OperationResult.Success();
    }

    /// <summary>
    /// Cria um reembolso para um checkout pago.
    /// </summary>
    public async Task<Result<CreateRefundResponse>> CreateRefundAsync(
        Guid checkoutId,
        Guid userId,
        long? amount,
        string? reason,
        CancellationToken cancellationToken)
    {
        var checkout = await GetCheckoutByIdAsync(checkoutId, cancellationToken);
        if (checkout is null)
        {
            return Result<CreateRefundResponse>.Failure("Checkout not found.");
        }

        if (checkout.BuyerUserId != userId)
        {
            return Result<CreateRefundResponse>.Failure("User is not authorized to refund this checkout.");
        }

        if (checkout.Status != CheckoutStatus.Paid)
        {
            return Result<CreateRefundResponse>.Failure("Only paid checkouts can be refunded.");
        }

        if (string.IsNullOrWhiteSpace(checkout.PaymentIntentId))
        {
            return Result<CreateRefundResponse>.Failure("Checkout does not have a payment intent ID.");
        }

        var refundResult = await _paymentGateway.CreateRefundAsync(
            checkout.PaymentIntentId,
            amount,
            reason,
            cancellationToken);

        // Atualizar status do checkout se necessário
        if (refundResult.Status == RefundStatus.Succeeded)
        {
            // Se reembolso total, pode marcar checkout como cancelado
            // Se parcial, manter como pago mas registrar reembolso
            // Por enquanto, apenas registramos o reembolso
        }

        await _checkoutRepository.UpdateAsync(checkout, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<CreateRefundResponse>.Success(new CreateRefundResponse(
            refundResult.RefundId,
            refundResult.Amount,
            refundResult.Status,
            checkoutId));
    }

    private async Task<Checkout?> GetCheckoutByIdAsync(Guid checkoutId, CancellationToken cancellationToken)
    {
        return await _checkoutRepository.GetByIdAsync(checkoutId, cancellationToken);
    }

    private async Task<Checkout?> GetCheckoutByPaymentIntentIdAsync(string paymentIntentId, CancellationToken cancellationToken)
    {
        return await _checkoutRepository.GetByPaymentIntentIdAsync(paymentIntentId, cancellationToken);
    }

    private static CheckoutStatus MapPaymentStatusToCheckoutStatus(PaymentStatus paymentStatus)
    {
        return paymentStatus switch
        {
            PaymentStatus.Succeeded => CheckoutStatus.Paid,
            PaymentStatus.Failed => CheckoutStatus.Canceled,
            PaymentStatus.Canceled => CheckoutStatus.Canceled,
            PaymentStatus.Pending => CheckoutStatus.AwaitingPayment,
            PaymentStatus.Processing => CheckoutStatus.AwaitingPayment,
            _ => CheckoutStatus.Canceled
        };
    }
}
