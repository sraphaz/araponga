using Arah.Application.Interfaces;
using Arah.Domain.Financial;

namespace Arah.Infrastructure.InMemory;

public sealed class InMemoryPlatformExpenseTransactionRepository : IPlatformExpenseTransactionRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryPlatformExpenseTransactionRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<PlatformExpenseTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = _dataStore.PlatformExpenseTransactions.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(transaction);
    }

    public Task<List<PlatformExpenseTransaction>> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.PlatformExpenseTransactions
            .Where(t => t.TerritoryId == territoryId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task<List<PlatformExpenseTransaction>> GetBySellerTransactionIdAsync(Guid sellerTransactionId, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.PlatformExpenseTransactions
            .Where(t => t.SellerTransactionId == sellerTransactionId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task AddAsync(PlatformExpenseTransaction transaction, CancellationToken cancellationToken)
    {
        _dataStore.PlatformExpenseTransactions.Add(transaction);
        return Task.CompletedTask;
    }
}
