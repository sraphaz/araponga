namespace Arah.Domain.Financial;

/// <summary>
/// Status de uma transação financeira.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Pendente (aguardando processamento).
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Processando (em processamento).
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Concluída (processada com sucesso).
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Falhou (processamento falhou).
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Cancelada (cancelada antes de processar).
    /// </summary>
    Canceled = 5
}
