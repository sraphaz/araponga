namespace Araponga.Domain.Financial;

/// <summary>
/// Histórico de mudanças de status de uma transação financeira.
/// </summary>
public sealed class TransactionStatusHistory
{
    public Guid Id { get; private set; }
    public Guid FinancialTransactionId { get; private set; }
    public TransactionStatus PreviousStatus { get; private set; }
    public TransactionStatus NewStatus { get; private set; }
    
    /// <summary>
    /// ID do usuário que fez a mudança (null se automático).
    /// </summary>
    public Guid? ChangedByUserId { get; private set; }
    
    /// <summary>
    /// Razão da mudança de status.
    /// </summary>
    public string? Reason { get; private set; }
    
    public DateTime ChangedAtUtc { get; private set; }
    
    // Navigation property
    public FinancialTransaction FinancialTransaction { get; private set; } = null!;
    
    private TransactionStatusHistory() { } // EF Core
    
    public TransactionStatusHistory(
        Guid id,
        Guid financialTransactionId,
        TransactionStatus previousStatus,
        TransactionStatus newStatus,
        Guid? changedByUserId = null,
        string? reason = null)
    {
        Id = id;
        FinancialTransactionId = financialTransactionId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        ChangedByUserId = changedByUserId;
        Reason = reason;
        ChangedAtUtc = DateTime.UtcNow;
    }
}
