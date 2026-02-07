namespace Arah.Domain.Financial;

/// <summary>
/// Tipo de transação financeira.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Transação de checkout (compra).
    /// </summary>
    Checkout = 1,

    /// <summary>
    /// Transação de pagamento (pagamento processado).
    /// </summary>
    Payment = 2,

    /// <summary>
    /// Transação de vendedor (valor devido ao vendedor).
    /// </summary>
    Seller = 3,

    /// <summary>
    /// Fee da plataforma (receita da plataforma).
    /// </summary>
    PlatformFee = 4,

    /// <summary>
    /// Payout (transferência para vendedor).
    /// </summary>
    Payout = 5,

    /// <summary>
    /// Reembolso (devolução de pagamento).
    /// </summary>
    Refund = 6
}
