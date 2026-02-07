using Arah.Domain.Financial;

namespace Arah.Application.Interfaces;

public interface ITransactionStatusHistoryRepository
{
    Task<List<TransactionStatusHistory>> GetByTransactionIdAsync(Guid financialTransactionId, CancellationToken cancellationToken);
    Task AddAsync(TransactionStatusHistory history, CancellationToken cancellationToken);
}
