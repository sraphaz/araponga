using Araponga.Application.Interfaces;
using Araponga.Domain.Financial;

namespace Araponga.Infrastructure.InMemory;

public sealed class InMemoryPlatformFinancialBalanceRepository : IPlatformFinancialBalanceRepository
{
    private readonly InMemoryDataStore _dataStore;

    public InMemoryPlatformFinancialBalanceRepository(InMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<PlatformFinancialBalance?> GetByTerritoryIdAsync(Guid territoryId, CancellationToken cancellationToken)
    {
        var balance = _dataStore.PlatformFinancialBalances.FirstOrDefault(b => b.TerritoryId == territoryId);
        return Task.FromResult(balance);
    }

    public Task<List<PlatformFinancialBalance>> GetAllAsync(CancellationToken cancellationToken)
    {
        var balances = _dataStore.PlatformFinancialBalances
            .OrderBy(b => b.TerritoryId)
            .ToList();
        return Task.FromResult(balances);
    }

    public Task AddAsync(PlatformFinancialBalance balance, CancellationToken cancellationToken)
    {
        _dataStore.PlatformFinancialBalances.Add(balance);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(PlatformFinancialBalance balance, CancellationToken cancellationToken)
    {
        var index = _dataStore.PlatformFinancialBalances.FindIndex(b => b.Id == balance.Id);
        if (index >= 0)
        {
            _dataStore.PlatformFinancialBalances[index] = balance;
        }
        return Task.CompletedTask;
    }
}
