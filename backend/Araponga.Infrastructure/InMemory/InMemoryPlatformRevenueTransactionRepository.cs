using Araponga.Application.Interfaces;
using Araponga.Domain.Financial;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryPlatformRevenueTransactionRepository : IPlatformRevenueTransactionRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryPlatformRevenueTransactionRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<PlatformRevenueTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = _dataStore.PlatformRevenueTransactions.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(transaction);
    }

    public Task<List<PlatformRevenueTransaction>> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.PlatformRevenueTransactions
            .Where(t => t.TerritoryId == territoryId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task<List<PlatformRevenueTransaction>> GetByCheckoutIdAsync(Guid checkoutId, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.PlatformRevenueTransactions
            .Where(t => t.CheckoutId == checkoutId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task AddAsync(PlatformRevenueTransaction transaction, CancellationToken cancellationToken)
    {
        _dataStore.PlatformRevenueTransactions.Add(transaction);
        return Task.CompletedTask;
    }
}
