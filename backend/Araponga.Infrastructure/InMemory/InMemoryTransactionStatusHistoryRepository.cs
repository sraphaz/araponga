using Araponga.Application.Interfaces;
using Araponga.Domain.Financial;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryTransactionStatusHistoryRepository : ITransactionStatusHistoryRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryTransactionStatusHistoryRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<List<TransactionStatusHistory>> GetByTransactionIdAsync(Guid financialTransactionId, CancellationToken cancellationToken)
    {
        var histories = _dataStore.TransactionStatusHistories
            .Where(h => h.FinancialTransactionId == financialTransactionId)
            .OrderBy(h => h.ChangedAtUtc)
            .ToList();
        return Task.FromResult(histories);
    }

    public Task AddAsync(TransactionStatusHistory history, CancellationToken cancellationToken)
    {
        _dataStore.TransactionStatusHistories.Add(history);
        return Task.CompletedTask;
    }
}
