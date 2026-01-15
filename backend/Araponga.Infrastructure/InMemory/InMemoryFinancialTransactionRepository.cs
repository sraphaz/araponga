using Araponga.Application.Interfaces;
using Araponga.Domain.Financial;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryFinancialTransactionRepository : IFinancialTransactionRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryFinancialTransactionRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<FinancialTransaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = _dataStore.FinancialTransactions.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(transaction);
    }

    public Task<List<FinancialTransaction>> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.FinancialTransactions
            .Where(t => t.TerritoryId == territoryId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task<List<FinancialTransaction>> GetByRelatedEntityIdAsync(Guid relatedEntityId, string? relatedEntityType, CancellationToken cancellationToken)
    {
        var query = _dataStore.FinancialTransactions
            .Where(t => t.RelatedEntityId == relatedEntityId);

        if (!string.IsNullOrWhiteSpace(relatedEntityType))
        {
            query = query.Where(t => t.RelatedEntityType == relatedEntityType);
        }

        var transactions = query
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task<List<FinancialTransaction>> GetByTypeAsync(Guid territoryId, TransactionType type, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.FinancialTransactions
            .Where(t => t.TerritoryId == territoryId && t.Type == type)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task<List<FinancialTransaction>> GetByStatusAsync(Guid territoryId, TransactionStatus status, CancellationToken cancellationToken)
    {
        var transactions = _dataStore.FinancialTransactions
            .Where(t => t.TerritoryId == territoryId && t.Status == status)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList();
        return Task.FromResult(transactions);
    }

    public Task AddAsync(FinancialTransaction transaction, CancellationToken cancellationToken)
    {
        _dataStore.FinancialTransactions.Add(transaction);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FinancialTransaction transaction, CancellationToken cancellationToken)
    {
        var index = _dataStore.FinancialTransactions.FindIndex(t => t.Id == transaction.Id);
        if (index >= 0)
        {
            _dataStore.FinancialTransactions[index] = transaction;
        }
        return Task.CompletedTask;
    }
}
